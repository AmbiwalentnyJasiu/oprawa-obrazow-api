using System.ComponentModel.DataAnnotations;

namespace OprawaObrazow.Modules.Auth.Requests;

public class PasswordChangeRequest
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = null!;
    
    [Required]
    public string OldPassword { get; set; } = null!;
    
    [Required]
    public string NewPassword { get; set; } = null!;
}