using HCM.Core.Models.Employee;
using HCM.Core.Models.Salary;
using HCM.Infrastructure.Entities;
using System.Text;

namespace HCM.Core.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDetails ToEmployeeDetails(this Employee employee, List<SalaryDetails>? salaryDetails = null)
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
                Salaries = salaryDetails ?? new List<SalaryDetails>()
            };

            return employeeDetails;
        }

        public static IEnumerable<EmployeeBasicInfo> ToEmployeeBasicInfo(this IEnumerable<Employee> employees)
        {
            var employeeBasicInfoList = employees.Select(e => new EmployeeBasicInfo()
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
    }
}
