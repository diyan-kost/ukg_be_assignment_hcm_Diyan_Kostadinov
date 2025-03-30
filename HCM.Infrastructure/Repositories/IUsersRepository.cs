using HCM.Infrastructure.Entities;

namespace HCM.Infrastructure.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> GetByUsernameAsync(string username);

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmployeeIdAsync(int employeeId);
    }
}
