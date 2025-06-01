using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data;
using OprawaObrazow.Data.Models;

namespace OprawaObrazow.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}

public class UserRepository(DatabaseContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
    }
}