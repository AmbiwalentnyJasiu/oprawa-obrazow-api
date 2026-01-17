using OprawaObrazow.Modules.FramePiece.Dto;
namespace OprawaObrazow.Modules.Frame.Dto;

public class FrameListDto
{
  public int Id { get; set; }
  public string Code { get; set; } = null!;
  public bool HasDecoration { get; set; }
  public string SupplierName { get; set; } = null!;
  public int Width { get; set; }
  public decimal Price { get; set; }
  public string ColorName { get; set; } = null!;
  public List<FramePieceViewDto> FramePieces { get; set; } = [];
}