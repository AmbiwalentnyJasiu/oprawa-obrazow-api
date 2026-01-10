using Microsoft.AspNetCore.Mvc;
using OprawaObrazow.Modules.Auth.Dto;
using OprawaObrazow.Modules.Auth.Requests;
using OprawaObrazow.Modules.Auth.Responses;

namespace OprawaObrazow.Modules.Auth;

[ApiController]
[Route("api-main/[controller]")]
public class AuthController(IAuthService authService, IJwtService jwtService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        try
        {
            var user = await authService.AuthenticateUserAsync(request.Username, request.Password);

            if (user is null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                });
            }

            string token = jwtService.GenerateJwtToken(user);

            return Ok(new LoginResponse
            {
                Success = true,
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username
                }
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while logging in");
            return BadRequest(new LoginResponse
            {
                Success = false,
                Message = "Error while logging in"
            });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
    {
        try
        {
            await authService.CreateUserAsync(request.Username, request.Password);

            return Ok(new RegisterResponse
            {
                Success = true,
                Message = $"User {request.Username} registered successfully"
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while registering user");
            return BadRequest(new RegisterResponse
            {
                Success = false,
                Message = "Error while registering user"
            });
        }
    }

    [HttpPost("changePassword")]
    public async Task<ActionResult<RegisterResponse>> ChangePassword(PasswordChangeRequest request)
    {
        try
        {
            var user = await authService.AuthenticateUserAsync(request.Username, request.OldPassword);
            
            if (user is null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                });
            }
            
            await authService.ChangePasswordAsync(user, request.NewPassword);
            
            return Ok(new RegisterResponse
            {
                Success = true,
                Message = $"Password for user {request.Username} changed successfully"
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while changing password");
            return BadRequest(new RegisterResponse
            {
                Success = false,
                Message = "Error while changing password"
            });
        }
    }
}