using OprawaObrazow.Modules.Base;

namespace OprawaObrazow.Modules.FramePiece.Dto;

public class FramePieceFiltersDto : BaseFiltersDto
{
  public int? FrameId { get; set; }
  public int? OrderId { get; set; }
  public bool? IsDamaged { get; set; }
  public bool? IsInStock { get; set; }
  public bool? IsOrdered { get; set; }
  public int? MinLength { get; set; }
  public int? MaxLength { get; set; }
}