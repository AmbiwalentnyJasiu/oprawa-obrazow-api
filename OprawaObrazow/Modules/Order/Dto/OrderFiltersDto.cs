using OprawaObrazow.Data.Order;
using OprawaObrazow.Modules.Base;

namespace OprawaObrazow.Modules.Order.Dto;

public class OrderFiltersDto : BaseFiltersDto
{
  public OrderStatus? Status { get; set; }
  public DateOnly? DateFrom { get; set; }
  public DateOnly? DateTo { get; set; }
}