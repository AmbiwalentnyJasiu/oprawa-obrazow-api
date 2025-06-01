using System.Linq.Expressions;

namespace OprawaObrazow.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? skip = null,
        int? take = null
    );
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByIdNoTrackingAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}