using HCM.Core.Mappers;
using HCM.Core.Models.Employee;
using HCM.Infrastructure.Repositories;

namespace HCM.Core.Services.Implementations
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ISalariesRepository _salariesRepository; 

        public EmployeesService(IEmployeesRepository employeesRepository, ISalariesRepository salariesRepository)
        {
            _employeesRepository = employeesRepository;
            _salariesRepository = salariesRepository;
        }

        public async Task<EmployeeDetails> GetEmployeeDetailsById(int id)
        {
            var employee = await _employeesRepository.GetByIdAsync(id);

            if (employee == null)
                throw new Exception("Employee not found");

            var salaries = await _salariesRepository.GetSalariesByEmployeeIdAsync(employee.Id);

            var salaryDetails = salaries.ToSalaryDetails();

            var employeeDetails = employee.ToEmployeeDetails(salaryDetails);

            return employeeDetails;
        }

        public async Task<IEnumerable<EmployeeBasicInfo>> GetEmployeesByManagerId(int managerId)
        {
            var employees = await _employeesRepository.GetByManagerIdAsync(managerId);

            var employeeBasicInfoList = employees.ToEmployeeBasicInfo();

            return employeeBasicInfoList;
        }

        public async Task<IEnumerable<EmployeeBasicInfo>> GetAllEmployees()
        {
            var employees = await _employeesRepository.GetAllAsync();

            var employeeBasicInfoList = employees.ToEmployeeBasicInfo();

            return employeeBasicInfoList;
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeModel model)
        {
            var employee = await _employeesRepository.GetByIdAsync(model.Id, false);

            if (employee == null)
                throw new Exception("Employee not found");

            employee.FirstName = model.FirstName;
            employee.MiddleName = model.MiddleName;
            employee.LastName = model.LastName;
            employee.ManagerId = model.ManagerId;
            employee.Email = model.Email;
            employee.PhoneNumber = model.PhoneNumber;
            employee.CurrentAddress = model.Address;
            employee.NationalIdNumber = model.NationalIdNumber;

            await _employeesRepository.SaveTrackingChangesAsync();
        }
    }
}
