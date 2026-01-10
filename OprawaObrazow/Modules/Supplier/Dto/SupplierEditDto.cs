using System.ComponentModel.DataAnnotations;

namespace OprawaObrazow.Modules.Supplier.Dto;

public class SupplierEditDto
{
  public int? Id { get; set; }

  [Required]
  [MaxLength( 200, ErrorMessage = "Nazwa dostawcy nie może przekraczać 200 znaków." )]
  public string Name { get; set; } = null!;
}