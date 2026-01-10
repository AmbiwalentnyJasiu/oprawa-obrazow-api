using System.Linq.Expressions;
using AutoMapper;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Supplier.Dto;

namespace OprawaObrazow.Modules.Supplier;

public interface ISupplierService : IBaseService<SupplierEditDto, SupplierViewDto, SupplierViewDto, SupplierFiltersDto>;

public class SupplierService( IBaseRepository<Data.Supplier.Supplier> repository, IMapper mapper )
  : BaseService<Data.Supplier.Supplier, SupplierEditDto, SupplierViewDto, SupplierViewDto, SupplierFiltersDto>(
    repository, mapper ), ISupplierService
{
  protected override Expression ApplyFilters( SupplierFiltersDto filters, ParameterExpression parameter )
  {
    Expression expression = Expression.Constant( true );

    if ( filters.Search is not null )
      expression = AddFilterExpression( expression, parameter, nameof( Data.Supplier.Supplier.Name ), filters.Search );

    return expression;
  }

  protected override Func<IQueryable<Data.Supplier.Supplier>, IOrderedQueryable<Data.Supplier.Supplier>>
    GetOrderByExpression( SupplierFiltersDto filters )
  {
    return OrderBy;

    IOrderedQueryable<Data.Supplier.Supplier> OrderBy( IQueryable<Data.Supplier.Supplier> query )
    {
      return ApplyOrder( query, c => c.Id, false );
    }
  }

  protected override async Task<(bool isValid, Dictionary<string, List<string>>? errors)> ValidateInputEntity(
  SupplierEditDto editModel )
  {
    var (_, totalCount) = await repository.GetAllAsync( s =>
      s.Name == editModel.Name && ( editModel.Id == null || s.Id != editModel.Id ) );

    if ( totalCount > 0 )
      return ( false, new Dictionary<string, List<string>>
      {
        { nameof( SupplierEditDto.Name ), ["Dostawca o tej nazwie już istnieje."] }
      } );

    return ( true, null );
  }
}