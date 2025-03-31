using HCM.Core.Models.Employee;

namespace HCM.Core.Services
{
    public interface IEmployeesService
    {
        Task<EmployeeDetails> GetEmployeeDetailsById(int id);

        Task<IEnumerable<EmployeeBasicInfo>> GetEmployeesByManagerId(int managerId);

        Task<IEnumerable<EmployeeBasicInfo>> GetAllEmployees();

        Task UpdateEmployeeAsync(UpdateEmployeeModel model);

        Task AddNewEmployeeAsync(AddNewEmployeeModel model);
    }
}
