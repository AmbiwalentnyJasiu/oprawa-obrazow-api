using System.Linq.Expressions;
using AutoMapper;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Frame.Dto;

namespace OprawaObrazow.Modules.Frame;

public interface IFrameService : IBaseService<FrameEditDto, FrameViewDto, FrameListDto, FrameFiltersDto>;

public class FrameService( IBaseRepository<Data.Frame.Frame> repository, IMapper mapper )
  : BaseService<Data.Frame.Frame, FrameEditDto, FrameViewDto, FrameListDto, FrameFiltersDto>( repository, mapper ),
    IFrameService
{
  public override async Task<FrameViewDto?> GetByIdAsync( int id )
  {
    var entity = await repository.GetByIdNoTrackingIncludeAsync( id, "Supplier", "Color", "FramePieces" );
    return entity is null ? null : mapper.Map<FrameViewDto>( entity );
  }

  public new async Task<BaseListResponse<FrameListDto>> GetAllAsync( FrameFiltersDto filters )
  {
    var filterExpression = GetFilterExpression( filters );
    var orderByExpression = GetOrderByExpression( filters );
    var skip = ( filters.Page - 1 ) * filters.PageSize;

    var (items, totalCount) =
      await repository.GetAllIncludeAsync( filterExpression, orderByExpression, skip, filters.PageSize,
        "Supplier", "Color", "FramePieces" );

    return new BaseListResponse<FrameListDto>
    {
      Count = totalCount,
      Items = items.Select( mapper.Map<FrameListDto> )
    };
  }

  protected override async Task<(bool isValid, Dictionary<string, List<string>>? errors)> ValidateInputEntity(
  FrameEditDto editModel )
  {
    var errors = new Dictionary<string, List<string>>();

    if ( editModel.Width < 0 ) errors.Add( nameof( FrameEditDto.Width ), ["Szerokość nie może być ujemna."] );

    if ( editModel.Price < 0 ) errors.Add( nameof( FrameEditDto.Price ), ["Cena nie może być ujemna."] );

    var (_, totalCount) = await repository.GetAllAsync( f =>
      f.Code == editModel.Code && ( editModel.Id == null || f.Id != editModel.Id ) );

    if ( totalCount > 0 )
    {
      if ( errors.TryGetValue( nameof( FrameEditDto.Code ), out var value ) )
        value.Add( "Rama o tym kodzie już istnieje." );
      else
        errors.Add( nameof( FrameEditDto.Code ), ["Rama o tym kodzie już istnieje."] );
    }

    return errors.Count > 0 ? ( false, errors ) : ( true, null );
  }

  protected override async Task<Data.Frame.Frame> MapForEdit( FrameEditDto editModel )
  {
    var entityId = editModel.Id ?? 0;
    if ( entityId is 0 )
      throw new ArgumentException( "Id must be set" );
    var currentEntity = await repository.GetByIdNoTrackingAsync( entityId );

    if ( currentEntity is null )
      throw new ArgumentException( "Entity with provided id not found" );

    currentEntity.Code = editModel.Code;
    currentEntity.HasDecoration = editModel.HasDecoration;
    currentEntity.SupplierId = editModel.SupplierId;
    currentEntity.Width = editModel.Width;
    currentEntity.Price = editModel.Price;
    currentEntity.ColorId = editModel.ColorId;

    return currentEntity;
  }

  protected override Expression ApplyFilters( FrameFiltersDto filters, ParameterExpression parameter )
  {
    Expression expression = Expression.Constant( true );

    if ( filters.Search is not null )
      expression = Expression.AndAlso( expression,
        AddOrFilterExpression( Expression.Constant( false ), parameter, nameof( Data.Frame.Frame.Code ),
          filters.Search ) );

    if ( filters.ColorId is not null )
    {
      var colorIdProperty = Expression.Property( parameter, nameof( Data.Frame.Frame.ColorId ) );
      expression = Expression.AndAlso( expression,
        Expression.Equal( colorIdProperty, Expression.Constant( filters.ColorId.Value ) ) );
    }

    if ( filters.SupplierId is not null )
    {
      var colorIdProperty = Expression.Property( parameter, nameof( Data.Frame.Frame.SupplierId ) );
      expression = Expression.AndAlso( expression,
        Expression.Equal( colorIdProperty, Expression.Constant( filters.SupplierId.Value ) ) );
    }

    if ( filters.MinWidth is not null )
    {
      var widthProperty = Expression.Property( parameter, nameof( Data.Frame.Frame.Width ) );
      expression = Expression.AndAlso( expression,
        Expression.GreaterThanOrEqual( widthProperty, Expression.Constant( filters.MinWidth.Value ) ) );
    }

    if ( filters.MaxWidth is not null )
    {
      var widthProperty = Expression.Property( parameter, nameof( Data.Frame.Frame.Width ) );
      expression = Expression.AndAlso( expression,
        Expression.LessThanOrEqual( widthProperty, Expression.Constant( filters.MaxWidth.Value ) ) );
    }

    if ( filters.HasDecoration is not null )
    {
      var hasDecorationProperty = Expression.Property( parameter, nameof( Data.Frame.Frame.HasDecoration ) );
      expression = Expression.AndAlso( expression,
        Expression.Equal( hasDecorationProperty, Expression.Constant( filters.HasDecoration.Value ) ) );
    }

    return expression;
  }

  protected override Func<IQueryable<Data.Frame.Frame>, IOrderedQueryable<Data.Frame.Frame>> GetOrderByExpression(
  FrameFiltersDto filters )
  {
    var sortSplit = filters.Sort.Split( ' ', StringSplitOptions.RemoveEmptyEntries );
    var descending = sortSplit.Length > 1 && sortSplit[1] == "desc";

    return OrderBy;

    IOrderedQueryable<Data.Frame.Frame> OrderBy( IQueryable<Data.Frame.Frame> query )
    {
      return sortSplit[0] switch
      {
        "id" => ApplyOrder( query, entity => entity.Id, descending ),
        "code" => ApplyOrder( query, entity => entity.Code, descending ),
        "width" => ApplyOrder( query, entity => entity.Width, descending ),
        "price" => ApplyOrder( query, entity => entity.Price, descending ),
        "hasDecoration" => ApplyOrder( query, entity => entity.HasDecoration, descending ),
        _ => ApplyOrder( query, entity => entity.Id, false )
      };
    }
  }
}