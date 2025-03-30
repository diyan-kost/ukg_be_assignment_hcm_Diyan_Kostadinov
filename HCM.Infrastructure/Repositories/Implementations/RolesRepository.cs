using HCM.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories.Implementations
{
    public class RolesRepository : IRolesRepository
    {
        private readonly HCMContext _dbContext;

        public RolesRepository(HCMContext dbContex) 
        { 
            _dbContext = dbContex;
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            var role = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == name);

            return role;
        }
    }
}
