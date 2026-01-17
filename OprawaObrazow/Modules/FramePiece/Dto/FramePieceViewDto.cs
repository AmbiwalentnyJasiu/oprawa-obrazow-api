namespace OprawaObrazow.Modules.FramePiece.Dto;

public class FramePieceViewDto
{
  public int Id { get; set; }
  public int Length { get; set; }
  public bool IsDamaged { get; set; }
  public string? StorageLocation { get; set; }
  public bool IsInStock { get; set; }
  public int? OrderId { get; set; }
  public int FrameId { get; set; }
  public string FrameCode { get; set; } = null!;
}