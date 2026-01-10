using OprawaObrazow.Data.Order;

namespace OprawaObrazow.Modules.Order.Dto;

public class OrderListDto
{
  public OrderListDto() { }

  public OrderListDto( Data.Order.Order order )
  {
    Id = order.Id;
    Price = order.Price;
    Status = order.Status;
    DateDue = order.DateDue;
    Dimensions = $"{order.PictureWidth}x{order.PictureHeight}";
    ClientName = order.ClientName;
    ContactData = GetContactData( order );
  }

  public int Id { get; set; }
  public decimal Price { get; set; }
  public OrderStatus Status { get; set; }
  public DateOnly DateDue { get; set; }
  public string Dimensions { get; set; } = null!;
  public string ClientName { get; set; } = null!;
  public string ContactData { get; set; } = null!;

  private string GetContactData( Data.Order.Order order )
  {
    if ( order.EmailAddress is not null && order.PhoneNumber is not null )
      return $"{order.PhoneNumber} | {order.EmailAddress}";
    if ( order.EmailAddress is not null && order.PhoneNumber is null )
      return $"{order.EmailAddress}";
    if ( order.EmailAddress is null && order.PhoneNumber is not null )
      return $"{order.PhoneNumber}";

    return "";
  }
}