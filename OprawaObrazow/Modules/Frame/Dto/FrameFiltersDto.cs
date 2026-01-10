using OprawaObrazow.Modules.Base;

namespace OprawaObrazow.Modules.Frame.Dto;

public class FrameFiltersDto : BaseFiltersDto
{
  public int? ColorId { get; set; }
  public int? SupplierId { get; set; }
  public int? MinWidth { get; set; }
  public int? MaxWidth { get; set; }
  public bool? HasDecoration { get; set; }
}