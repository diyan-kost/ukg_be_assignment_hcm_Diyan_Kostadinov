using HCM.Core.Models.User;
using HCM.Infrastructure.Entities;

namespace HCM.Core.Services
{
    public interface IUsersService
    {
        Task<int> LoginAsync(LoginUserModel loginUserModel);

        Task LogoutAsync();

        Task<string?> GetUsernameByEmployeeIdAsync(int employeeId);

        Task CreateUserAsync(CreateUser model);

        Task UpdateUserAsync(UpdateUser model);

        Task DeleteUserAsync(string username);

        Task<string> GetUserRoleByUsernameAsync(string username);
    }
}
