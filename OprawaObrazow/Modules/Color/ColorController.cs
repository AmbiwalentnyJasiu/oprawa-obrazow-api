using Microsoft.AspNetCore.Mvc;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Color.Dto;

namespace OprawaObrazow.Modules.Color;

[ApiController]
[Route( "api-main/[controller]" )]
public class ColorController( IColorService colorService, ILogger<ColorController> logger )
  : BaseController<IColorService, ColorEditDto, ColorViewDto, ColorViewDto, ColorFiltersDto>( colorService, logger )
{
  protected override string GetEntityName()
  {
    return "color";
  }
}