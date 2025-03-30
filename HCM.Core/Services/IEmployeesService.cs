using HCM.Core.Models.Employee;

namespace HCM.Core.Services
{
    public interface IEmployeesService
    {
        Task<EmployeeDetails> GetEmployeeDetailsById(int id);
    }
}
