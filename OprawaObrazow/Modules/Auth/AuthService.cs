using OprawaObrazow.Data.User;

namespace OprawaObrazow.Modules.Auth;

public interface IAuthService
{
    Task<User?> AuthenticateUserAsync(string username, string password);
    Task CreateUserAsync(string username, string password);
    Task ChangePasswordAsync(User user, string newPassword);

}

public class AuthService(IUserRepository userRepository, IPasswordHashService passwordHashService) : IAuthService
{
    public async Task<User?> AuthenticateUserAsync(string username, string password)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        
        if(user == null || user.IsDeleted) return null;
        
        var isPasswordValid = passwordHashService.VerifyPassword(password, user.Password, user.PasswordSalt);
        
        return isPasswordValid ? user : null;
    }

    public async Task CreateUserAsync(string username, string password)
    {
        var salt = passwordHashService.GenerateSalt();

        var hashedPassword = passwordHashService.HashPassword(password, salt);

        var user = new User
        {
            Username = username,
            Password = hashedPassword,
            PasswordSalt = salt
        };
        
        await userRepository.AddAsync(user);
    }

    public async Task ChangePasswordAsync(User user, string newPassword)
    {
        var newHashedPassword = passwordHashService.HashPassword(newPassword, user.PasswordSalt);

        var newUser = new User
        {
            Id = user.Id,
            IsDeleted = user.IsDeleted,
            PasswordSalt = user.PasswordSalt,
            Username = user.Username,
            Password = newHashedPassword
        };
        
        await userRepository.UpdateAsync(newUser);
    }
}