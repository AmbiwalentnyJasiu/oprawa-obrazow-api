using Microsoft.AspNetCore.Mvc;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.FramePiece.Dto;

namespace OprawaObrazow.Modules.FramePiece;

[ApiController]
[Route( "api-main/[controller]" )]
public class FramePieceController( IFramePieceService framePieceService, ILogger<FramePieceController> fpLogger )
  : BaseController<IFramePieceService, FramePieceEditDto, FramePieceViewDto, FramePieceViewDto, FramePieceFiltersDto>(
    framePieceService, fpLogger )
{
  [HttpPost( "{id:int}/attach-to-order/{orderId:int}" )]
  public async Task<IActionResult> AttachToOrder( int id, int orderId )
  {
    try
    {
      await service.AttachToOrder( id, orderId );
      return Ok();
    }
    catch ( ArgumentException ex )
    {
      return BadRequest( ex.Message );
    }
    catch ( Exception ex )
    {
      logger.LogError( ex, "Error attaching frame piece {Id} to order {OrderId}", id, orderId );
      return StatusCode( 500, "Internal server error" );
    }
  }

  [HttpPost( "{id:int}/detach-from-order" )]
  public async Task<IActionResult> DetachFromOrder( int id )
  {
    try
    {
      await service.DetachFromOrder( id );
      return Ok();
    }
    catch ( ArgumentException ex )
    {
      return BadRequest( ex.Message );
    }
    catch ( Exception ex )
    {
      logger.LogError( ex, "Error detaching frame piece {Id} from order", id );
      return StatusCode( 500, "Internal server error" );
    }
  }

  protected override string GetEntityName() => "frame-piece";
}