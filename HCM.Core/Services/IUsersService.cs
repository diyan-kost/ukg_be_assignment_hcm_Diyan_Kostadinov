using HCM.Core.Models.User;
using HCM.Infrastructure.Entities;

namespace HCM.Core.Services
{
    public interface IUsersService
    {
        Task<int> LoginAsync(LoginUserDto loginUserModel);

        Task LogoutAsync();

        Task<string?> GetUsernameByEmployeeIdAsync(int employeeId);

        Task CreateUserAsync(CreateUserDto model);

        Task UpdateUserAsync(UpdateUserDto model);

        Task DeleteUserAsync(string username);

        Task<string> GetUserRoleByUsernameAsync(string username);
    }
}
