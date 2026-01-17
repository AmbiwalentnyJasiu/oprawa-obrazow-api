using System.ComponentModel.DataAnnotations;

namespace OprawaObrazow.Modules.Frame.Dto;

public class FrameEditDto
{
  public int? Id { get; set; }

  [Required]
  [MaxLength( 20, ErrorMessage = "Kod ramy nie może przekraczać 20 znaków." )]
  public string Code { get; set; } = null!;

  [Required]
  public bool HasDecoration { get; set; }

  [Required]
  public int SupplierId { get; set; }

  [Required]
  [Range( 1, int.MaxValue, ErrorMessage = "Szerokość nie może być ujemna." )]
  public int Width { get; set; }

  [Required]
  [Range( 0, 100000, ErrorMessage = "Cena nie może być ujemna." )]
  public decimal Price { get; set; }

  [Required]
  public int ColorId { get; set; }
}