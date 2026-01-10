using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OprawaObrazow.Data.User;

namespace OprawaObrazow.Modules.Auth;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key not found in configuration"));
        var keyLifetimeHours = int.Parse(configuration["Jwt:KeyLifetimeHours"] ?? throw new Exception("Jwt:KeyLifetimeHours not found in configuration"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddHours(keyLifetimeHours),
            Issuer = configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer not found in configuration"),
            Audience = configuration["Jwt:Audience"] ?? throw new Exception("Jwt:Audience not found in configuration"),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}