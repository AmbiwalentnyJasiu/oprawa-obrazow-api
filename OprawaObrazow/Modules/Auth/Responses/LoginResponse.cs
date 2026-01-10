using OprawaObrazow.Modules.Auth.Dto;

namespace OprawaObrazow.Modules.Auth.Responses;

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public UserDto? User { get; set; }
}