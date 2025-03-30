using HCM.Infrastructure.Entities;

namespace HCM.Infrastructure.Repositories
{
    public interface IEmployeesRepository
    {
        Task<Employee?> GetByIdAsync(int id);

        Task<IEnumerable<Employee>> GetByManagerIdAsync(int managerId);

        Task<IEnumerable<Employee>> GetAllAsync();
    }
}
