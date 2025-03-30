using HCM.Core.Models.Employee;
using HCM.Core.Models.Salary;
using HCM.Infrastructure.Repositories;
using Microsoft.CodeAnalysis.Operations;
using System.Text;

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

            var managerName = "None";
            if (employee.Manager != null)
            {

                var fullName = new StringBuilder(employee.Manager.FirstName);

                if (!string.IsNullOrEmpty(employee.Manager.MiddleName))
                    fullName.Append($" {employee.Manager.MiddleName}");

                if (!string.IsNullOrEmpty(employee.Manager.LastName))
                    fullName.Append($" {employee.Manager.LastName}");

                managerName = fullName.ToString();
            }

            var salaries = await _salariesRepository.GetSalariesByEmployeeIdAsync(employee.Id);

            var employeeDetails = new EmployeeDetails()
            {
                FirstName = employee.FirstName,
                MiddleName = employee.LastName,
                LastName = employee.LastName,
                NationalIdNumber = employee.NationalIdNumber,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                CurrentAddress = employee.CurrentAddress,
                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender,
                HiredAt = employee.HiredAt,
                ManagerName = managerName,
                ManagerId = employee.ManagerId,
                Salaries = salaries
                .Select(s => new SalaryDetails { Amount = s.Amount, EffectiveDate = s.EffectiveDate, Note = s.Note })
                .ToList()
            };

            return employeeDetails;
        }

        public async Task<IEnumerable<EmployeeBasicInfo>> GetEmployeesByManagerId(int managerId)
        {
            var employees = await _employeesRepository.GetByManagerIdAsync(managerId);

            var employeeBasicInfoList = employees.Select(e => new EmployeeBasicInfo()
            {
                Id = e.Id,
                FullName = $"{e.FirstName} {e.MiddleName} {e.LastName}",
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HiredAt = e.HiredAt,
            });

            return employeeBasicInfoList;
        }

        public async Task<IEnumerable<EmployeeBasicInfo>> GetAllEmployees()
        {
            var employees = await _employeesRepository.GetAllAsync();

            var employeeBasicInfoList = employees.Select(e => new EmployeeBasicInfo()
            {
                Id = e.Id,
                FullName = $"{e.FirstName} {e.MiddleName} {e.LastName}",
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HiredAt = e.HiredAt,
                Manager = e.Manager != null ? $"{e.Manager.FirstName} {e.MiddleName} {e.LastName}" : null
            });

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
