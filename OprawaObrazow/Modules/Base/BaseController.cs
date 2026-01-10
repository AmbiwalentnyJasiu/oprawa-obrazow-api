using Microsoft.AspNetCore.Mvc;

namespace OprawaObrazow.Modules.Base;

public abstract class BaseController<TService, TEditDto, TViewDto, TListDto, TFiltersDto>(
  TService service,
  ILogger logger )
  : ControllerBase
  where TService : IBaseService<TEditDto, TViewDto, TListDto, TFiltersDto>
  where TEditDto : class
  where TViewDto : class
  where TListDto : class
  where TFiltersDto : BaseFiltersDto
{
  protected readonly ILogger logger = logger;
  protected readonly TService service = service;

  [HttpGet]
  public virtual async Task<ActionResult<BaseListResponse<TListDto>>> GetAllAsync( [FromQuery] TFiltersDto filters )
  {
    try
    {
      var result = await service.GetAllAsync( filters );
      return Ok( result );
    }
    catch ( Exception ex )
    {
      return HandleError( ex, GetErrorMessage( "getting multiple" ) );
    }
  }

  [HttpGet( "{id:int}" )]
  public virtual async Task<ActionResult<TViewDto>> GetByIdAsync( int id )
  {
    if ( id <= 0 ) return BadRequest( "Invalid id" );

    try
    {
      var entity = await service.GetByIdAsync( id );
      if ( entity is null ) return NotFound();

      return Ok( entity );
    }
    catch ( Exception ex )
    {
      return HandleError( ex, GetErrorMessage( "getting single" ) );
    }
  }

  [HttpPost]
  public virtual async Task<ActionResult> AddAsync( TEditDto dto )
  {
    try
    {
      await service.AddAsync( dto );
      return Created();
    }
    catch ( Exception ex )
    {
      return HandleError( ex, GetErrorMessage( "adding" ) );
    }
  }

  [HttpPut]
  public virtual async Task<ActionResult> UpdateAsync( TEditDto dto )
  {
    try
    {
      await service.UpdateAsync( dto );
      return Ok();
    }
    catch ( Exception ex )
    {
      return HandleError( ex, GetErrorMessage( "updating" ) );
    }
  }

  [HttpDelete]
  public virtual async Task<ActionResult> DeleteAsync( int id )
  {
    try
    {
      if ( id <= 0 ) return BadRequest();
      await service.DeleteAsync( id );
      return Ok();
    }
    catch ( Exception ex )
    {
      return HandleError( ex, GetErrorMessage( "deleting" ) );
    }
  }

  protected abstract string GetEntityName();

  private string GetErrorMessage( string operation ) => $"Error while {operation} {GetEntityName()}";

  protected ObjectResult HandleError( Exception ex, string message )
  {
    var traceId = HttpContext.TraceIdentifier;
    logger.LogError( ex, "TraceId: {traceId}\t\t{message}", traceId, message );
    return StatusCode( 500, new { error = message, requestId = traceId } );
  }
}