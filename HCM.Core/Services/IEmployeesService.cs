using HCM.Core.Models.Employee;

namespace HCM.Core.Services
{
    public interface IEmployeesService
    {
        Task<EmployeeDetailsDto> GetEmployeeDetailsByIdAsync(int id);

        Task<IEnumerable<EmployeeInfoDto>> GetEmployeesByManagerIdAsync(int managerId);

        Task<IEnumerable<EmployeeInfoDto>> GetAllEmployeesAsync();

        Task UpdateEmployeeAsync(UpdateEmployeeDto model);

        Task AddNewEmployeeAsync(AddNewEmployeeDto model);

        Task DeleteEmployeeAsync(int id);
    }
}
