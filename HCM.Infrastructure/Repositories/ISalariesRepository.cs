using HCM.Infrastructure.Entities;

namespace HCM.Infrastructure.Repositories
{
    public interface ISalariesRepository
    {
        Task<IEnumerable<Salary>> GetSalariesByEmployeeIdAsync(int employeeId);

        Task<Salary> AddNewSalaryAsync(Salary salary);
    }
}
