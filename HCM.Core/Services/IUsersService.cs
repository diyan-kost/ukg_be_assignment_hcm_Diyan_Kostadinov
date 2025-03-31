using HCM.Core.Models.User;

namespace HCM.Core.Services
{
    public interface IUsersService
    {
        Task<int> LoginAsync(LoginUserModel loginUserModel);

        Task LogoutAsync();

        Task<string?> GetUsernameByEmployeeIdAsync(int employeeId);

        Task CreateUserAsync(CreateUser model);

        Task UpdateUserAsync(UpdateUser model);

        Task DeleteUserAsync(DeleteUser model);

        Task<string> GetUserRoleByUsernameAsync(string username);
    }
}
