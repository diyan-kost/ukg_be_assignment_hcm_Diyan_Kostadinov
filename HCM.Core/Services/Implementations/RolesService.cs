using HCM.Core.Mappers;
using HCM.Core.Models.Role;
using HCM.Infrastructure.Repositories;

namespace HCM.Core.Services.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<IEnumerable<RoleIdDto>> GetRolesAsync()
        {
            var roles = await _rolesRepository.GetAllAsync();

            return roles.ToRoleInfos();
        }
    }
}
