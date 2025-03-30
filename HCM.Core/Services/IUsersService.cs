using HCM.Core.Models.User;

namespace HCM.Core.Services
{
    public interface IUsersService
    {
        Task<int> LoginAsync(LoginUserModel loginUserModel);

        Task LogoutAsync();

        Task<string?> GetUsernameByEmployeeIdAsync(int employeeId);
    }
}
