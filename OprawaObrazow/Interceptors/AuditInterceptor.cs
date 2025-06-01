using System.Text.Json;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OprawaObrazow.Data;
using OprawaObrazow.Data.AuditModels;
using OprawaObrazow.Data.Models;

namespace OprawaObrazow.Interceptors;

public class AuditInterceptor(AuditContext auditContext, ILogger<AuditInterceptor> logger) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = new())
    {
        var context = eventData.Context;
        if (context is null) return result;

        try
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            var timeStamp = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                await CreateAuditEntryAsync(entry, timeStamp, cancellationToken);
            }
        }
        catch (NotSupportedException nse)
        {
            logger.LogError(nse, "Unsupported entity type or state");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating audit entry");
            auditContext.ChangeTracker.Clear();
        }
        
        return result;
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new())
    {
        if (!auditContext.ChangeTracker.HasChanges())
        {
            return result;
        }
        
        using var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions{IsolationLevel = IsolationLevel.ReadCommitted}, TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            await auditContext.SaveChangesAsync(cancellationToken);

            transactionScope.Complete();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving audit entries");
            auditContext.ChangeTracker.Clear();
            throw;
        }

        return result;
    }
    
    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        logger.LogWarning("Main SaveChanges failed, clearing audit context");
        auditContext.ChangeTracker.Clear();
        base.SaveChangesFailed(eventData);
    }
    
    private async Task CreateAuditEntryAsync(EntityEntry entry, DateTime timeStamp, CancellationToken cancellationToken)
    {
        var entityType = entry.Entity.GetType();
        var entityName = entityType.Name;

        var changeType = entry.State switch
        {
            EntityState.Added => "INSERT",
            EntityState.Deleted => "DELETE",
            EntityState.Modified => "UPDATE",
            _ => throw new NotSupportedException($"Unsupported entity state: {entry.State}")
        };
        
        var keyName = entry.Metadata.FindPrimaryKey()!.Properties[0].Name;
        var recordId = (int)entry.Property(keyName).CurrentValue!;

        string entityData;
        if (entry.State is EntityState.Deleted)
        {
            var originalValues = new Dictionary<string, object?>();
            foreach (var property in entry.Properties)
            {
                originalValues.Add(property.Metadata.Name, property.OriginalValue);
            }
            entityData = JsonSerializer.Serialize(originalValues);
        }
        else
        {
            entityData = JsonSerializer.Serialize(entry.Entity);
        }

        switch (entityName)
        {
            case "Client":
                auditContext.Clients.Add(new ClientAudit
                {
                    ChangeType = changeType,
                    ChangedAt = timeStamp,
                    RecordId = recordId,
                    EntityData = entityData
                });
                break;
            case "Delivery":
                auditContext.Deliveries.Add(new DeliveryAudit
                {
                    ChangeType = changeType,
                    ChangedAt = timeStamp,
                    RecordId = recordId,
                    EntityData = entityData
                });
                break;
            case "Frame":
                auditContext.Frames.Add(new FrameAudit
                {
                    ChangeType = changeType,
                    ChangedAt = timeStamp,
                    RecordId = recordId,
                    EntityData = entityData
                });
                break;
            case "FramePiece":
                auditContext.FramePieces.Add(new FramePieceAudit
                {
                    ChangeType = changeType,
                    ChangedAt = timeStamp,
                    RecordId = recordId,
                    EntityData = entityData
                });
                break;
            case "Order":
                auditContext.Orders.Add(new OrderAudit
                {
                    ChangeType = changeType,
                    ChangedAt = timeStamp,
                    RecordId = recordId,
                    EntityData = entityData
                });
                break;
            case "Supplier":
                auditContext.Suppliers.Add(new SupplierAudit
                {
                    ChangeType = changeType,
                    ChangedAt = timeStamp,
                    RecordId = recordId,
                    EntityData = entityData
                });
                break;
            case "User":
                auditContext.Users.Add(new UserAudit
                {
                    ChangeType = changeType,
                    ChangedAt = timeStamp,
                    RecordId = recordId,
                    EntityData = entityData
                });
                break;
            default:
                throw new NotSupportedException($"Unsupported entity type: {entityName}");
        }
    }
}