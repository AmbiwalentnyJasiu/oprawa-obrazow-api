using System.ComponentModel.DataAnnotations;

namespace OprawaObrazow.Modules.FramePiece.Dto;

public class FramePieceEditDto
{
  public int? Id { get; set; }

  [Required]
  [Range( 1, int.MaxValue, ErrorMessage = "Długość musi być większa od 0" )]
  public int Length { get; set; }

  [Required]
  public bool IsDamaged { get; set; }

  [Required]
  [MaxLength( 10, ErrorMessage = "Maksymalna długość lokalizacji to 10 znaków" )]
  public string? StorageLocation { get; set; }

  [Required]
  public bool IsInStock { get; set; }

  [Required]
  public int FrameId { get; set; }
}