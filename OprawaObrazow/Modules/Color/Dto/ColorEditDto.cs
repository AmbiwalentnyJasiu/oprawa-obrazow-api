using System.ComponentModel.DataAnnotations;

namespace OprawaObrazow.Modules.Color.Dto;

public class ColorEditDto
{
  public int? Id { get; set; }

  [Required]
  [MaxLength( 30, ErrorMessage = "Nazwa koloru nie może przekraczać 30 znaków." )]
  public string Name { get; set; } = null!;
}