using System.ComponentModel.DataAnnotations;

namespace OprawaObrazow.Modules.Auth.Requests;

public class LoginRequest
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}