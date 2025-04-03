using HCM.Core.Models.Employee;
using HCM.Core.Models.Salary;
using HCM.Infrastructure.Entities;
using System.Text;

namespace HCM.Core.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDetailsDto ToEmployeeDetails(this Employee employee, List<SalaryDto>? salaryDetails = null)
        {
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

            var employeeDetails = new EmployeeDetailsDto()
            {
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
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
                Salaries = salaryDetails ?? new List<SalaryDto>()
            };

            return employeeDetails;
        }

        public static IEnumerable<EmployeeInfoDto> ToEmployeeBasicInfo(this IEnumerable<Employee> employees)
        {
            var employeeBasicInfoList = employees.Select(e => new EmployeeInfoDto()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                MiddleName = e.MiddleName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HiredAt = e.HiredAt,
                ManagerFirstName = e.Manager?.FirstName,
                ManagerMiddleName = e.Manager?.MiddleName,
                ManagerLastName = e.Manager?.LastName,
            });

            return employeeBasicInfoList;
        }

        public static Employee ToEmployee(this AddNewEmployeeDto model)
        {
            var employee = new Employee()
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                HiredAt = model.StartingDate,
                CurrentAddress = model.Address,
                DateOfBirth = model.DateOfBirth,
                IsEuCitizen = model.IsEuCitizen,
                Gender = model.Gender,
                NationalIdNumber = model.NationalIdNumber,
                ManagerId = model.ManagerId,
            };

            return employee;
        }
    }
}
