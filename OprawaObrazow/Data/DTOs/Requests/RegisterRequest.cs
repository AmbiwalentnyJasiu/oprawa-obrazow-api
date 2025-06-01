using System.ComponentModel.DataAnnotations;

namespace OprawaObrazow.Data.DTOs.Requests;

public class RegisterRequest
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}