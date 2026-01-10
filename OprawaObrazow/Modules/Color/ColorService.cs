using System.Linq.Expressions;
using AutoMapper;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Color.Dto;

namespace OprawaObrazow.Modules.Color;

public interface IColorService : IBaseService<ColorEditDto, ColorViewDto, ColorViewDto, ColorFiltersDto>;

public class ColorService( IBaseRepository<Data.Color.Color> repository, IMapper mapper )
  : BaseService<Data.Color.Color, ColorEditDto, ColorViewDto, ColorViewDto, ColorFiltersDto>( repository, mapper ),
    IColorService
{
  protected override Expression ApplyFilters( ColorFiltersDto filters, ParameterExpression parameter )
  {
    Expression expression = Expression.Constant( true );

    if ( filters.Search is not null )
      expression = AddFilterExpression( expression, parameter, nameof( Data.Color.Color.Name ), filters.Search );

    return expression;
  }

  protected override Func<IQueryable<Data.Color.Color>, IOrderedQueryable<Data.Color.Color>> GetOrderByExpression(
  ColorFiltersDto filters )
  {
    return OrderBy;

    IOrderedQueryable<Data.Color.Color> OrderBy( IQueryable<Data.Color.Color> query )
    {
      return ApplyOrder( query, entity => entity.Name, false );
    }
  }

  protected override async Task<(bool isValid, Dictionary<string, List<string>>? errors)> ValidateInputEntity(
  ColorEditDto editModel )
  {
    var (_, totalCount) = await repository.GetAllAsync( c =>
      c.Name == editModel.Name && ( editModel.Id == null || c.Id != editModel.Id ) );

    if ( totalCount > 0 )
      return ( false, new Dictionary<string, List<string>>
      {
        { nameof( ColorEditDto.Name ), ["Kolor o tej nazwie już istnieje."] }
      } );

    return ( true, null );
  }
}