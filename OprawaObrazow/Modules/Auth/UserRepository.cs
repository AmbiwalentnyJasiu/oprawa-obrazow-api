using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data;
using OprawaObrazow.Data.User;
using OprawaObrazow.Modules.Base;

namespace OprawaObrazow.Modules.Auth;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}

public class UserRepository(DatabaseContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    private readonly DatabaseContext _dbContext = dbContext;

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
    }
}