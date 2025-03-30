using HCM.Core.Helpers;
using HCM.Core.Models.User;
using HCM.Infrastructure.Entities;

namespace HCM.Core.Mappers
{
    public static class UserMapper
    {
        public static User ToUser(this CreateUser createUser, int roleId)
        {
            var user = new User()
            {
                Username = createUser.Username,
                Password_Hash = LoginHelper.ComputeSHA256Hash(createUser.Password),
                RoleId = roleId,
                EmployeeId = createUser.EmployeeId,
            };

            return user;
        }
    }
}
