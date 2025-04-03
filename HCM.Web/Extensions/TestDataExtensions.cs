using HCM.Core.Helpers;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure;
using HCM.Web.Common;

namespace HCM.Web.Extensions
{
    public static class TestData
    {
        public static void InitializeDbWithTestData(HCMContext dbContext, bool deletePreviousData = true)
        {
            if (deletePreviousData)
            {
                dbContext.Salaries.RemoveRange(dbContext.Salaries);
                dbContext.Employees.RemoveRange(dbContext.Employees);
                dbContext.Users.RemoveRange(dbContext.Users);
                dbContext.Roles.RemoveRange(dbContext.Roles);

                dbContext.SaveChanges();
            }

            var employeeRole = new Role { Name = LoginRoles.EMPLOYEE, Description = "Can only view and edit his own data" };
            var managerRole = new Role { Name = LoginRoles.MANAGER, Description = "Can view and edit (some of) the data of the employees he manages" };
            var hrAdminRole = new Role { Name = LoginRoles.HR_ADMIN, Description = "Can create, edit, delete and view all employees' data" };

            dbContext.Roles.Add(employeeRole);
            dbContext.Roles.Add(managerRole);
            dbContext.Roles.Add(hrAdminRole);

            dbContext.SaveChanges();

            var employee1 = new Employee
            {
                FirstName = "Miroslav",
                LastName = "Zahariev",
                CurrentAddress = "Lorem ipsum",
                DateOfBirth = DateTime.Today.AddYears(-44),
                PhoneNumber = "55555551",
                Email = "miroslav.zahariev@hcm.com",
                Gender = "Male",
                NationalIdNumber = "111111111"
            };

            dbContext.Employees.Add(employee1);
            dbContext.SaveChanges();

            var employee2 = new Employee
            {
                FirstName = "Yavor",
                LastName = "Alexandrov",
                CurrentAddress = "Lorem ipsum",
                DateOfBirth = DateTime.Today.AddYears(-30),
                PhoneNumber = "55555552",
                Email = "yavor.alexandrov@hcm.com",
                Gender = "Male",
                NationalIdNumber = "111111112",
                ManagerId = employee1.Id
            };

            dbContext.Employees.Add(employee2);
            dbContext.SaveChanges();

            var employee3 = new Employee
            {
                FirstName = "Nataliya",
                LastName = "Stoeva",
                CurrentAddress = "Lorem ipsum",
                DateOfBirth = DateTime.Today.AddYears(-23),
                PhoneNumber = "55555553",
                Email = "nataliya.stoeva@hcm.com",
                Gender = "Female",
                NationalIdNumber = "111111113",
                ManagerId = employee2.Id
            };

            dbContext.Employees.Add(employee3);
            dbContext.SaveChanges();

            var hrAdminUser = new User()
            {
                Username = "miroslav.zahariev",
                Password_Hash = LoginHelper.ComputeSHA256Hash("1"),
                EmployeeId = employee1.Id,
                RoleId = hrAdminRole.Id,
            };

            var managerUser = new User()
            {
                Username = "yavor.alexandrov",
                Password_Hash = LoginHelper.ComputeSHA256Hash("1"),
                EmployeeId = employee2.Id,
                RoleId = managerRole.Id,
            };

            var employeeUser = new User()
            {
                Username = "nataliya.stoeva",
                Password_Hash = LoginHelper.ComputeSHA256Hash("1"),
                EmployeeId = employee1.Id,
                RoleId = employeeRole.Id,
            };

            dbContext.Users.Add(hrAdminUser);
            dbContext.Users.Add(managerUser);
            dbContext.Users.Add(employeeUser);

            dbContext.SaveChanges();

            var hrAdminSalary1 = new Salary()
            {
                Amount = 4500,
                EffectiveDate = DateTime.Now.AddYears(-2),
                Note = "Starting salary",
                EmployeeId = employee1.Id,
            };

            var hrAdminSalary2 = new Salary()
            {
                Amount = 5500,
                EffectiveDate = DateTime.Now.AddYears(-1),
                EmployeeId = employee1.Id,
            };

            var managerSalary = new Salary()
            {
                Amount = 3400,
                EffectiveDate = DateTime.Now.AddMonths(-6),
                EmployeeId = employee2.Id
            };

            var employeeSalary = new Salary()
            {
                Amount = 2500,
                EffectiveDate = DateTime.Now.AddMonths(-1),
                EmployeeId = employee3.Id
            };

            dbContext.Salaries.Add(hrAdminSalary1);
            dbContext.Salaries.Add(hrAdminSalary2);
            dbContext.Salaries.Add(managerSalary);
            dbContext.Salaries.Add(employeeSalary);

            dbContext.SaveChanges();
        }
    }
}
