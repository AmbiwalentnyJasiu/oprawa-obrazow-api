using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Modules.Base;

public interface IBaseService<in TEditDto, TViewDto, TListDto, in TFiltersDto>
  where TEditDto : class
  where TViewDto : class
  where TListDto : class
  where TFiltersDto : BaseFiltersDto
{
  Task<BaseListResponse<TListDto>> GetAllAsync( TFiltersDto filters );

  Task<TViewDto?> GetByIdAsync( int id );

  Task AddAsync( TEditDto editModel );

  Task UpdateAsync( TEditDto editModel );

  Task DeleteAsync( int id );
}

public abstract class BaseService<TEntity, TEditDto, TViewDto, TListDto, TFiltersDto>(
  IBaseRepository<TEntity> repo,
  IMapper mapper ) : IBaseService<TEditDto, TViewDto, TListDto, TFiltersDto>
  where TEntity : BaseEntity
  where TEditDto : class
  where TViewDto : class
  where TListDto : class
  where TFiltersDto : BaseFiltersDto
{
  protected readonly IMapper mapper = mapper;
  protected readonly IBaseRepository<TEntity> repository = repo;

  public async Task AddAsync( TEditDto editModel )
  {
    var (isValid, errors) = await ValidateInputEntity( editModel );
    if ( !isValid )
      throw new ArgumentException( "Validation failed: " + string.Join( ", ", errors!.SelectMany( e => e.Value ) ) );

    var entity = mapper.Map<TEntity>( editModel );
    await repository.AddAsync( entity );
  }

  public async Task UpdateAsync( TEditDto editModel )
  {
    var (isValid, errors) = await ValidateInputEntity( editModel );
    if ( !isValid )
      throw new ArgumentException( "Validation failed: " + string.Join( ", ", errors!.SelectMany( e => e.Value ) ) );

    var entity = await MapForEdit( editModel );
    await repository.UpdateAsync( entity );
  }

  public async Task DeleteAsync( int id )
  {
    var entity = await repository.GetByIdAsync( id );
    if ( entity == null ) return;
    await repository.DeleteAsync( entity );
  }

  public virtual async Task<TViewDto?> GetByIdAsync( int id )
  {
    var entity = await repository.GetByIdNoTrackingAsync( id );
    return entity is null ? null : mapper.Map<TViewDto>( entity );
  }

  public async Task<BaseListResponse<TListDto>> GetAllAsync( TFiltersDto filters )
  {
    var filterExpression = GetFilterExpression( filters );
    var orderByExpression = GetOrderByExpression( filters );
    var skip = ( filters.Page - 1 ) * filters.PageSize;

    var (items, totalCount) =
      await repository.GetAllAsync( filterExpression, orderByExpression, skip, filters.PageSize );

    return new BaseListResponse<TListDto>
    {
      Count = totalCount,
      Items = items.Select( mapper.Map<TListDto> )
    };
  }

  protected Expression<Func<TEntity, bool>> GetFilterExpression( TFiltersDto filters )
  {
    var parameter = Expression.Parameter( typeof( TEntity ), "entity" );
    var expression = ApplyFilters( filters, parameter );

    return Expression.Lambda<Func<TEntity, bool>>( expression, parameter );
  }

  protected IOrderedQueryable<TEntity> ApplyOrder<TKey>( IQueryable<TEntity> query,
  Expression<Func<TEntity, TKey>> keySelector, bool descending ) =>
    descending ? query.OrderByDescending( keySelector ) : query.OrderBy( keySelector );

  protected Expression AddFilterExpression( Expression currentExpression, ParameterExpression parameter,
  string propertyName, string filterValue )
  {
    var containsMethod = GetContainsMethod();
    return Expression.AndAlso( currentExpression, Expression.Call( Expression.Property( parameter, propertyName ),
      containsMethod, Expression.Constant( filterValue ) ) );
  }

  protected Expression AddOrFilterExpression( Expression currentExpression, ParameterExpression parameter,
  string propertyName, string filterValue )
  {
    var containsMethod = GetContainsMethod();
    return Expression.Or( currentExpression, Expression.Call( Expression.Property( parameter, propertyName ),
      containsMethod, Expression.Constant( filterValue ) ) );
  }

  private static MethodInfo GetContainsMethod()
  {
    return typeof( string ).GetRuntimeMethods()
                           .Single( m =>
                             m.Name == nameof( string.Contains ) &&
                             m.GetParameters().Length == 1 &&
                             m.GetParameters()[0].ParameterType == typeof( string ) );
  }

  protected virtual async Task<TEntity> MapForEdit( TEditDto editModel ) => mapper.Map<TEntity>( editModel );

  protected virtual Expression ApplyFilters( TFiltersDto filters, ParameterExpression parameter ) =>
    Expression.Constant( true );

  protected virtual Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderByExpression( TFiltersDto filters )
  {
    return OrderBy;

    IOrderedQueryable<TEntity> OrderBy( IQueryable<TEntity> query )
    {
      return ApplyOrder( query, entity => entity.Id, false );
    }
  }

  protected virtual Task<(bool isValid, Dictionary<string, List<string>>? errors)> ValidateInputEntity(
  TEditDto editModel ) =>
    Task.FromResult<(bool, Dictionary<string, List<string>>?)>( ( true, null ) );
}