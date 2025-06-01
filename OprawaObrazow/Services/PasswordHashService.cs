using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace OprawaObrazow.Services;

public interface IPasswordHashService
{
    string GenerateSalt();
    string HashPassword(string password, string salt);
    bool VerifyPassword(string providedPassword, string storedHash, string storedSalt);

}

public class PasswordHashService : IPasswordHashService
{
    private const int IterationCount = 10000;
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;
    
    public string GenerateSalt()
    {
        var salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return Convert.ToBase64String(salt);
    }

    public string HashPassword(string password, string salt)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
        
        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentNullException(nameof(salt));
        
        var saltBytes = Convert.FromBase64String(salt);
        
        var hash = KeyDerivation.Pbkdf2(
            password,
            saltBytes,
            Prf,
            IterationCount,
            HashSize);
        
        return Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string providedPassword, string storedHash, string storedSalt)
    {
        if (string.IsNullOrWhiteSpace(providedPassword) || string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
            return false;
        
        string computedHash = HashPassword(providedPassword, storedSalt);

        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(storedHash), 
            Convert.FromBase64String(computedHash));
    }
}