using HCM.Core.Models.Employee;

namespace HCM.Core.Services
{
    public interface IEmployeesService
    {
        Task<EmployeeDetails> GetEmployeeDetailsByIdAsync(int id);

        Task<IEnumerable<EmployeeBasicInfo>> GetEmployeesByManagerIdAsync(int managerId);

        Task<IEnumerable<EmployeeBasicInfo>> GetAllEmployeesAsync();

        Task UpdateEmployeeAsync(UpdateEmployeeModel model);

        Task AddNewEmployeeAsync(AddNewEmployeeModel model);

        Task DeleteEmployeeAsync(int id);
    }
}
