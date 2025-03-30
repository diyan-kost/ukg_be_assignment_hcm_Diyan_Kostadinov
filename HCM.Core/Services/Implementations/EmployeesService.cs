using HCM.Core.Models.Employee;
using HCM.Core.Models.Salary;
using HCM.Infrastructure.Repositories;
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
            if (employee.ManagerId != null)
            {
                var manager = await _employeesRepository.GetByIdAsync(employee.ManagerId.Value);

                var fullName = new StringBuilder(manager.FirstName);

                if (!string.IsNullOrEmpty(manager.MiddleName))
                    fullName.Append($" {manager.MiddleName}");

                if (!string.IsNullOrEmpty(manager.LastName))
                    fullName.Append($" {manager.LastName}");

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
                Salaries = salaries
                .Select(s => new SalaryDetails { Amount = s.Amount, EffectiveDate = s.EffectiveDate, Note = s.Note })
                .ToList()
            };

            return employeeDetails;
        }
    }
}
