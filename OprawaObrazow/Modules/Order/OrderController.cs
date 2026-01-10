using Microsoft.AspNetCore.Mvc;
using OprawaObrazow.Data.Order;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Order.Dto;

namespace OprawaObrazow.Modules.Order;

[ApiController]
[Route( "api-main/[controller]" )]
public class OrderController( IOrderService orderService, ILogger<OrderController> logger )
  : BaseController<IOrderService, OrderEditDto, OrderViewDto, OrderListDto, OrderFiltersDto>( orderService, logger )
{
  protected override string GetEntityName() => "order";

  [HttpPost( "change-status/{id:int}/{status:alpha}" )]
  public async Task<IActionResult> ChangeStatus( int id, string status )
  {
    if ( !Enum.TryParse<OrderStatus>( status, out var newStatus ) )
      return BadRequest( "Invalid status provided" );

    await service.ChangeStatus( id, newStatus );
    return Ok();
  }
}