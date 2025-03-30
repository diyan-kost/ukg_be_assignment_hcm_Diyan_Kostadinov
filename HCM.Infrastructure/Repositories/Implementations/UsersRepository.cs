using HCM.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly HCMContext _dbContext;

        public UsersRepository(HCMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }
    }
}
