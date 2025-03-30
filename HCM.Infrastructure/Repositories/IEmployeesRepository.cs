using HCM.Infrastructure.Entities;

namespace HCM.Infrastructure.Repositories
{
    public interface IEmployeesRepository
    {
        Task<Employee?> GetByIdAsync(int id);
    }
}
