using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data;

namespace OprawaObrazow.Modules.Base;

public interface IBaseRepository<T> where T : class
{
  Task<(IEnumerable<T> items, int totalCount)> GetAllAsync(
  Expression<Func<T, bool>>? filter = null,
  Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
  int? skip = null,
  int? take = null,
  params Expression<Func<T, object>>[] includes
  );

  Task<T?> GetByIdAsync( int id );
  Task<T?> GetByIdNoTrackingAsync( int id, params Expression<Func<T, object>>[] includes );
  Task AddAsync( T entity );
  Task UpdateAsync( T entity );
  Task DeleteAsync( T entity );
}

public class BaseRepository<T>( DatabaseContext dbContext ) : IBaseRepository<T>
  where T : class
{
  private readonly DbSet<T> _dbSet = dbContext.Set<T>();

  public async Task<(IEnumerable<T> items, int totalCount)> GetAllAsync( Expression<Func<T, bool>>? filter = null,
  Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? skip = null, int? take = null,
  params Expression<Func<T, object>>[] includes )
  {
    var query = _dbSet.AsNoTracking();

    if ( includes.Length != 0 ) query = includes.Aggregate( query, ( current, include ) => current.Include( include ) );

    if ( filter is not null ) query = query.Where( filter );

    var totalCount = await query.CountAsync();

    if ( orderBy is not null ) query = orderBy( query );

    if ( skip is not null ) query = query.Skip( skip.Value );

    if ( take is not null ) query = query.Take( take.Value );

    var items = await query.ToListAsync();

    return ( items, totalCount );
  }

  public async Task<T?> GetByIdAsync( int id ) => await _dbSet.FindAsync( id );

  public async Task<T?> GetByIdNoTrackingAsync( int id, params Expression<Func<T, object>>[] includes )
  {
    var query = _dbSet.AsNoTracking();

    if ( includes.Length != 0 ) query = includes.Aggregate( query, ( current, include ) => current.Include( include ) );

    return await query.FirstOrDefaultAsync( e => EF.Property<int>( e, "Id" ) == id );
  }

  public async Task AddAsync( T entity )
  {
    await _dbSet.AddAsync( entity );
    await dbContext.SaveChangesAsync();
  }

  public async Task UpdateAsync( T entity )
  {
    _dbSet.Update( entity );
    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteAsync( T entity )
  {
    _dbSet.Remove( entity );
    await dbContext.SaveChangesAsync();
  }
}