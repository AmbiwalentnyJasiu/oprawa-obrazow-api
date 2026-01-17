using System.ComponentModel.DataAnnotations;

namespace OprawaObrazow.Modules.Order.Dto;

public class OrderEditDto
{
  public int? Id { get; set; }

  [Range( 0, 100000, ErrorMessage = "Cena nie może być ujemna." )]
  public decimal Price { get; set; }

  [Required]
  public DateOnly DateDue { get; set; }

  [Required]
  [MaxLength( 200, ErrorMessage = "Nazwa klienta nie może przekraczać 200 znaków." )]
  public string ClientName { get; set; } = null!;

  [Required]
  [Range( 1, int.MaxValue, ErrorMessage = "Szerokość nie może być ujemna." )]
  public int PictureWidth { get; set; }

  [Required]
  [Range( 1, int.MaxValue, ErrorMessage = "Wysokość nie może być ujemna." )]
  public int PictureHeight { get; set; }

  [EmailAddress( ErrorMessage = "E-mail nie jest poprawny." )]
  [MaxLength( 50, ErrorMessage = "Adres e-mail nie może przekraczać 50 znaków." )]
  public string? EmailAddress { get; set; }

  [Phone]
  [RegularExpression( @"^\d{9,15}$", ErrorMessage = "Numer telefonu musi składać się z co najmniej 9 cyfr." )]
  public string? PhoneNumber { get; set; }
}