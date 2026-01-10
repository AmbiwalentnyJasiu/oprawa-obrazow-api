using System.Linq.Expressions;
using AutoMapper;
using OprawaObrazow.Data.Order;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Order.Dto;

namespace OprawaObrazow.Modules.Order;

public interface IOrderService : IBaseService<OrderEditDto, OrderViewDto, OrderListDto, OrderFiltersDto>
{
  Task ChangeStatus( int id, OrderStatus newStatus );
}

public class OrderService( IBaseRepository<Data.Order.Order> repository, IMapper mapper )
  : BaseService<Data.Order.Order, OrderEditDto, OrderViewDto, OrderListDto, OrderFiltersDto>( repository, mapper ),
    IOrderService
{
  public async Task ChangeStatus( int id, OrderStatus newStatus )
  {
    var currentEntity = await repository.GetByIdNoTrackingAsync( id );

    if ( currentEntity is null )
      throw new ArgumentException( "Entity with provided id not found" );

    currentEntity.Status = newStatus;
    await repository.UpdateAsync( currentEntity );
  }

  public override async Task<OrderViewDto?> GetByIdAsync( int id )
  {
    var entity = await repository.GetByIdNoTrackingAsync( id, order => order.FramePieces );
    return entity is null ? null : mapper.Map<OrderViewDto>( entity );
  }

  protected override async Task<Data.Order.Order> MapForEdit( OrderEditDto editModel )
  {
    var entityId = editModel.Id ?? 0;
    if ( entityId is 0 )
      throw new ArgumentException( "Id must be set" );
    var currentEntity = await repository.GetByIdNoTrackingAsync( entityId );

    if ( currentEntity is null )
      throw new ArgumentException( "Entity with provided id not found" );

    currentEntity.ClientName = editModel.ClientName;
    currentEntity.DateDue = editModel.DateDue;
    currentEntity.EmailAddress = editModel.EmailAddress;
    currentEntity.PhoneNumber = editModel.PhoneNumber;
    currentEntity.Price = editModel.Price;
    currentEntity.PictureHeight = editModel.PictureHeight;
    currentEntity.PictureWidth = editModel.PictureWidth;

    return currentEntity;
  }

  protected override Expression ApplyFilters( OrderFiltersDto filters, ParameterExpression parameter )
  {
    Expression expression = Expression.Constant( true );

    Expression searchExpression = Expression.Constant( false );

    if ( filters.Search is not null )
    {
      searchExpression = AddOrFilterExpression( searchExpression, parameter, nameof( Data.Order.Order.ClientName ),
        filters.Search );
      searchExpression = AddOrFilterExpression( searchExpression, parameter, nameof( Data.Order.Order.EmailAddress ),
        filters.Search );
      searchExpression = AddOrFilterExpression( searchExpression, parameter, nameof( Data.Order.Order.PhoneNumber ),
        filters.Search );
      expression = Expression.AndAlso( expression, searchExpression );
    }

    if ( filters.Status is not null )
    {
      var statusProperty = Expression.Property( parameter, nameof( Data.Order.Order.Status ) );
      expression = Expression.AndAlso( expression,
        Expression.Equal( statusProperty, Expression.Constant( filters.Status ) ) );
    }

    if ( filters.DateFrom is not null )
    {
      var dateDueProperty = Expression.Property( parameter, nameof( Data.Order.Order.DateDue ) );
      expression = Expression.AndAlso( expression,
        Expression.GreaterThanOrEqual( dateDueProperty, Expression.Constant( filters.DateFrom.Value ) ) );
    }

    if ( filters.DateTo is not null )
    {
      var dateDueProperty = Expression.Property( parameter, nameof( Data.Order.Order.DateDue ) );
      expression = Expression.AndAlso( expression,
        Expression.LessThanOrEqual( dateDueProperty, Expression.Constant( filters.DateTo.Value ) ) );
    }

    return expression;
  }

  protected override Func<IQueryable<Data.Order.Order>, IOrderedQueryable<Data.Order.Order>> GetOrderByExpression(
  OrderFiltersDto filters )
  {
    var sortSplit = filters.Sort.Split( ' ', StringSplitOptions.RemoveEmptyEntries );
    var descending = sortSplit.Length > 1 && sortSplit[1] == "desc";

    return OrderBy;

    IOrderedQueryable<Data.Order.Order> OrderBy( IQueryable<Data.Order.Order> query )
    {
      return sortSplit[0] switch
      {
        "id" => ApplyOrder( query, entity => entity.Id, descending ),
        "status" => ApplyOrder( query, entity => entity.Status, descending ),
        "dateDue" => ApplyOrder( query, entity => entity.DateDue, descending ),
        "price" => ApplyOrder( query, entity => entity.Price, descending ),
        "clientName" => ApplyOrder( query, entity => entity.ClientName, descending ),
        _ => ApplyOrder( query, entity => entity.Id, false )
      };
    }
  }
}