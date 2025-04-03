using HCM.Core.Models.Role;
using HCM.Infrastructure.Entities;

namespace HCM.Core.Mappers
{
    public static class RoleMapper
    {
        public static IEnumerable<RoleIdDto> ToRoleInfos(this IEnumerable<Role> roles)
        {
            return roles.Select(r => new RoleIdDto { Id = r.Id, Name = r.Name, });
        }
    }
}
