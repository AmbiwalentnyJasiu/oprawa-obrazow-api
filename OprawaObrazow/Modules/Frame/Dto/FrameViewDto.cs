using OprawaObrazow.Modules.FramePiece.Dto;

namespace OprawaObrazow.Modules.Frame.Dto;

public class FrameViewDto
{
  public int Id { get; set; }
  public int ColorId { get; set; }
  public decimal Price { get; set; }
  public int SupplierId { get; set; }
  public int Width { get; set; }
  public bool HasDecoration { get; set; }
  public string Code { get; set; } = null!;
  public List<FramePieceViewDto> FramePieces { get; set; } = [];
}