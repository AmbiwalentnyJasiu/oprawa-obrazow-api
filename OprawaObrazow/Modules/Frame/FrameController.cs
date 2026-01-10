using Microsoft.AspNetCore.Mvc;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Frame.Dto;

namespace OprawaObrazow.Modules.Frame;

[ApiController]
[Route( "api-main/[controller]" )]
public class FrameController( IFrameService frameService, ILogger<FrameController> logger )
  : BaseController<IFrameService, FrameEditDto, FrameViewDto, FrameListDto, FrameFiltersDto>( frameService, logger )
{
  protected override string GetEntityName() => "frame";
}