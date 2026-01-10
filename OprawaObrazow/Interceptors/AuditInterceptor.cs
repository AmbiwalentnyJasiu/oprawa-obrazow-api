using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OprawaObrazow.Data;
using OprawaObrazow.Data.Base;
using OprawaObrazow.Data.Color;
using OprawaObrazow.Data.Frame;
using OprawaObrazow.Data.FramePiece;
using OprawaObrazow.Data.Order;
using OprawaObrazow.Data.Supplier;
using OprawaObrazow.Data.User;

namespace OprawaObrazow.Interceptors;

public class AuditInterceptor( AuditContext auditContext, ILogger<AuditInterceptor> logger ) : SaveChangesInterceptor
{
  private readonly ConcurrentDictionary<DbContext, List<(EntityEntry Entry, EntityState State)>> _entitiesBeforeSave =
    new();

  private bool _auditInProgress;

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync( DbContextEventData eventData,
  InterceptionResult<int> result, CancellationToken cancellationToken = new() )
  {
    if ( _auditInProgress ) return new ValueTask<InterceptionResult<int>>( result );

    var context = eventData.Context;
    if ( context is null ) return new ValueTask<InterceptionResult<int>>( result );

    var entries = context.ChangeTracker.Entries()
                         .Where( e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted )
                         .Select( e => ( e, e.State ) )
                         .ToList();

    _entitiesBeforeSave[context] = entries;

    return new ValueTask<InterceptionResult<int>>( result );
  }

  public override async ValueTask<int> SavedChangesAsync( SaveChangesCompletedEventData eventData, int result,
  CancellationToken cancellationToken = new() )
  {
    if ( _auditInProgress ) return result;

    var context = eventData.Context;
    if ( context is null ) return result;

    try
    {
      _auditInProgress = true;


      if ( _entitiesBeforeSave.TryRemove( context, out var entries ) && entries.Count > 0 )
      {
        var timeStamp = DateTime.UtcNow;

        foreach ( var (entry, state) in entries ) CreateAuditEntry( entry, state, timeStamp );

        await auditContext.SaveChangesAsync( cancellationToken );
      }
    }
    catch ( NotSupportedException nse )
    {
      logger.LogError( nse, "Unsupported entity type or state" );
      auditContext.ChangeTracker.Clear();
    }
    catch ( Exception ex )
    {
      logger.LogError( ex, "Error saving audit entries" );
      auditContext.ChangeTracker.Clear();
      throw;
    }
    finally
    {
      _auditInProgress = false;
      _entitiesBeforeSave.Clear();
    }

    return result;
  }

  private void CreateAuditEntry( EntityEntry entry, EntityState state, DateTime timeStamp )
  {
    var changeType = state switch
    {
      EntityState.Added => "INSERT",
      EntityState.Deleted => "DELETE",
      EntityState.Modified => "UPDATE",
      _ => throw new NotSupportedException( $"Unsupported entity state: {entry.State}" )
    };

    string entityData;
    if ( entry.State is EntityState.Deleted )
    {
      var originalValues = new Dictionary<string, object?>();
      foreach ( var property in entry.Properties ) originalValues.Add( property.Metadata.Name, property.OriginalValue );
      entityData = JsonSerializer.Serialize( originalValues );
    }
    else
    {
      entityData = JsonSerializer.Serialize( entry.Entity );
    }

    BaseAudit auditEntry = entry.Entity switch
    {
      Frame frame => new FrameAudit
      {
        ChangeType = changeType,
        ChangedAt = timeStamp,
        RecordId = frame.Id,
        EntityData = entityData
      },

      FramePiece framePiece => new FramePieceAudit
      {
        ChangeType = changeType,
        ChangedAt = timeStamp,
        RecordId = framePiece.Id,
        EntityData = entityData
      },

      Order order => new OrderAudit
      {
        ChangeType = changeType,
        ChangedAt = timeStamp,
        RecordId = order.Id,
        EntityData = entityData
      },

      Supplier supplier => new SupplierAudit
      {
        ChangeType = changeType,
        ChangedAt = timeStamp,
        RecordId = supplier.Id,
        EntityData = entityData
      },

      User user => new UserAudit
      {
        ChangeType = changeType,
        ChangedAt = timeStamp,
        RecordId = user.Id,
        EntityData = entityData
      },

      Color color => new ColorAudit
      {
        ChangeType = changeType,
        ChangedAt = timeStamp,
        RecordId = color.Id,
        EntityData = entityData
      },

      _ => throw new NotSupportedException( $"Unsupported entity type: {entry.Entity.GetType().Name}" )
    };

    auditContext.Add( auditEntry );
  }
}