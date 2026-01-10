using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null) return new ValueTask<InterceptionResult<int>>(result);

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if(entry is not {State: EntityState.Deleted, Entity: ISoftDelete softDelete}) continue;
            
            entry.State = EntityState.Modified;
            softDelete.Delete();
        }
        
        return new ValueTask<InterceptionResult<int>>(result);
    }
}