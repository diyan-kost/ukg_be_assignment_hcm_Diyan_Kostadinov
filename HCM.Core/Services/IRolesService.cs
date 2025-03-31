using HCM.Core.Models.Role;

namespace HCM.Core.Services
{
    public interface IRolesService
    {
        Task<IEnumerable<RoleInfo>> GetRolesAsync();
    }
}
