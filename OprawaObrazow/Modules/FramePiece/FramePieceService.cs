using System.Linq.Expressions;
using AutoMapper;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.FramePiece.Dto;

namespace OprawaObrazow.Modules.FramePiece;

public interface
  IFramePieceService : IBaseService<FramePieceEditDto, FramePieceViewDto, FramePieceViewDto, FramePieceFiltersDto>
{
  Task AttachToOrder( int id, int orderId );
  Task DetachFromOrder( int id );
}

public class FramePieceService( IBaseRepository<Data.FramePiece.FramePiece> repository, IMapper mapper )
  : BaseService<Data.FramePiece.FramePiece, FramePieceEditDto, FramePieceViewDto, FramePieceViewDto,
    FramePieceFiltersDto>( repository, mapper ), IFramePieceService
{
  public override async Task<FramePieceViewDto?> GetByIdAsync( int id )
  {
    var entity = await repository.GetByIdNoTrackingIncludeAsync( id, "Frame" );
    return entity is null ? null : mapper.Map<FramePieceViewDto>( entity );
  }

  public new async Task<BaseListResponse<FramePieceViewDto>> GetAllAsync( FramePieceFiltersDto filters )
  {
    var filterExpression = GetFilterExpression( filters );
    var orderByExpression = GetOrderByExpression( filters );
    var skip = ( filters.Page - 1 ) * filters.PageSize;

    var (items, totalCount) =
      await repository.GetAllIncludeAsync( filterExpression, orderByExpression, skip, filters.PageSize, "Frame" );

    return new BaseListResponse<FramePieceViewDto>
    {
      Count = totalCount,
      Items = items.Select( mapper.Map<FramePieceViewDto> )
    };
  }

  public async Task AttachToOrder( int id, int orderId )
  {
    var entity = await repository.GetByIdAsync( id );
    if ( entity is null )
      throw new ArgumentException( "Frame piece with provided id not found" );

    entity.OrderId = orderId;
    await repository.UpdateAsync( entity );
  }

  public async Task DetachFromOrder( int id )
  {
    var entity = await repository.GetByIdAsync( id );
    if ( entity is null )
      throw new ArgumentException( "Frame piece with provided id not found" );

    entity.OrderId = null;
    await repository.UpdateAsync( entity );
  }

  protected override async Task<Data.FramePiece.FramePiece> MapForEdit( FramePieceEditDto editModel )
  {
    var entityId = editModel.Id ?? 0;
    if ( entityId is 0 )
      throw new ArgumentException( "Id must be set" );
    var currentEntity = await repository.GetByIdNoTrackingAsync( entityId );

    if ( currentEntity is null )
      throw new ArgumentException( "Entity with provided id not found" );

    currentEntity.Length = editModel.Length;
    currentEntity.IsDamaged = editModel.IsDamaged;
    currentEntity.StorageLocation = editModel.StorageLocation;
    currentEntity.IsInStock = editModel.IsInStock;
    currentEntity.FrameId = editModel.FrameId;

    return currentEntity;
  }

  protected override Expression ApplyFilters( FramePieceFiltersDto filters, ParameterExpression parameter )
  {
    Expression expression = Expression.Constant( true );

    if ( filters.FrameId is not null )
    {
      var frameIdProperty = Expression.Property( parameter, nameof( Data.FramePiece.FramePiece.FrameId ) );
      expression = Expression.AndAlso( expression,
        Expression.Equal( frameIdProperty, Expression.Constant( filters.FrameId.Value ) ) );
    }

    if ( filters.OrderId is not null )
    {
      var orderIdProperty = Expression.Property( parameter, nameof( Data.FramePiece.FramePiece.OrderId ) );
      expression = Expression.AndAlso( expression,
        Expression.Equal( orderIdProperty, Expression.Constant( filters.OrderId, typeof( int? ) ) ) );
    }

    if ( filters.IsDamaged is not null )
    {
      var isDamagedProperty = Expression.Property( parameter, nameof( Data.FramePiece.FramePiece.IsDamaged ) );
      expression = Expression.AndAlso( expression,
        Expression.Equal( isDamagedProperty, Expression.Constant( filters.IsDamaged.Value ) ) );
    }

    if ( filters.IsInStock is not null )
    {
      var isInStockProperty = Expression.Property( parameter, nameof( Data.FramePiece.FramePiece.IsInStock ) );
      expression = Expression.AndAlso( expression,
        Expression.Equal( isInStockProperty, Expression.Constant( filters.IsInStock.Value ) ) );
    }

    if ( filters.IsOrdered is not null )
    {
      var orderIdProperty = Expression.Property( parameter, nameof( Data.FramePiece.FramePiece.OrderId ) );
      var isOrderedExpression = filters.IsOrdered.Value
        ? Expression.NotEqual( orderIdProperty, Expression.Constant( null, typeof( int? ) ) )
        : Expression.Equal( orderIdProperty, Expression.Constant( null, typeof( int? ) ) );

      expression = Expression.AndAlso( expression, isOrderedExpression );
    }

    if ( filters.MinLength is not null )
    {
      var lengthProperty = Expression.Property( parameter, nameof( Data.FramePiece.FramePiece.Length ) );
      expression = Expression.AndAlso( expression,
        Expression.GreaterThanOrEqual( lengthProperty, Expression.Constant( filters.MinLength.Value ) ) );
    }

    if ( filters.MaxLength is not null )
    {
      var lengthProperty = Expression.Property( parameter, nameof( Data.FramePiece.FramePiece.Length ) );
      expression = Expression.AndAlso( expression,
        Expression.LessThanOrEqual( lengthProperty, Expression.Constant( filters.MaxLength.Value ) ) );
    }

    return expression;
  }

  protected override Func<IQueryable<Data.FramePiece.FramePiece>, IOrderedQueryable<Data.FramePiece.FramePiece>>
    GetOrderByExpression( FramePieceFiltersDto filters )
  {
    var sortSplit = filters.Sort.Split( ' ', StringSplitOptions.RemoveEmptyEntries );
    var descending = sortSplit.Length > 1 && sortSplit[1] == "desc";

    return OrderBy;

    IOrderedQueryable<Data.FramePiece.FramePiece> OrderBy( IQueryable<Data.FramePiece.FramePiece> query )
    {
      return sortSplit[0] switch
      {
        "id" => ApplyOrder( query, entity => entity.Id, descending ),
        "length" => ApplyOrder( query, entity => entity.Length, descending ),
        "storageLocation" => ApplyOrder( query, entity => entity.StorageLocation, descending ),
        "isInStock" => ApplyOrder( query, entity => entity.IsInStock, descending ),
        "isDamaged" => ApplyOrder( query, entity => entity.IsDamaged, descending ),
        _ => ApplyOrder( query, entity => entity.Id, false )
      };
    }
  }

  protected override async Task<(bool isValid, Dictionary<string, List<string>>? errors)> ValidateInputEntity(
  FramePieceEditDto editModel )
  {
    var errors = new Dictionary<string, List<string>>();

    if ( editModel.Length <= 0 )
      errors.Add( nameof( FramePieceEditDto.Length ), ["Długość musi być większa od zera."] );

    return errors.Count > 0 ? ( false, errors ) : ( true, null );
  }
}