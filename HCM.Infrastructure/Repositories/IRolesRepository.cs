using HCM.Infrastructure.Entities;

namespace HCM.Infrastructure.Repositories
{
    public interface IRolesRepository
    {
        Task<Role?> GetByNameAsync(string name);

        Task<Role?> GetByIdAsync(int id);

        Task<IEnumerable<Role>> GetAllAsync();
    }
}
