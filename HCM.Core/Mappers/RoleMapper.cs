using HCM.Core.Models.Role;
using HCM.Infrastructure.Entities;

namespace HCM.Core.Mappers
{
    public static class RoleMapper
    {
        public static IEnumerable<RoleInfo> ToRoleInfos(this IEnumerable<Role> roles)
        {
            return roles.Select(r => new RoleInfo { Id = r.Id, Name = r.Name, });
        }
    }
}
