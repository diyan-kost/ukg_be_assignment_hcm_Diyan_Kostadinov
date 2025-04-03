using HCM.Core.Helpers;
using HCM.Infrastructure;
using HCM.Infrastructure.Entities;

namespace HCM.Web.IntegrationTests.Helpers
{
    public class TestDbHelper
    {
        public static void InitializeDbForTests(HCMContext db)
        {
            db.Employees.RemoveRange(db.Employees);
            db.Users.RemoveRange(db.Users);
            db.Roles.RemoveRange(db.Roles);

            db.Roles.Add(new Role { Id = 1, Name = "Employee" });
            db.Roles.Add(new Role { Id = 2, Name = "Manager" });
            db.Roles.Add(new Role { Id = 3, Name = "Admin" });

            db.Employees.Add(new Employee { Id = 1, FirstName = "Test", CurrentAddress = "Test", DateOfBirth = DateTime.Today.AddYears(-18), PhoneNumber = "55555551", Email = "test@test.com", Gender = "Male", NationalIdNumber = "1111111" });
            db.Employees.Add(new Employee { Id = 2, FirstName = "Test", CurrentAddress = "Test", DateOfBirth = DateTime.Today.AddYears(-18), PhoneNumber = "55555552", Email = "test2@test.com", Gender = "Male", NationalIdNumber = "1111112" });
            db.Employees.Add(new Employee { Id = 3, FirstName = "Test", CurrentAddress = "Test", DateOfBirth = DateTime.Today.AddYears(-18), PhoneNumber = "55555553", Email = "test3@test.com", Gender = "Male", NationalIdNumber = "1111113" });

            db.Users.Add(new User() { Id = 1, Username = "test.employee", Password_Hash = LoginHelper.ComputeSHA256Hash("1"), EmployeeId = 1, RoleId = 1 });
            db.Users.Add(new User() { Id = 2, Username = "test.manager", Password_Hash = LoginHelper.ComputeSHA256Hash("1"), EmployeeId = 2, RoleId = 2 });
            db.Users.Add(new User() { Id = 3, Username = "test.admin", Password_Hash = LoginHelper.ComputeSHA256Hash("1"), EmployeeId = 3, RoleId = 3 });

            db.SaveChanges();
        }

        public static string GetValidEmployeeUsername(HCMContext db)
        {
            return db.Users.First(u => u.RoleId == 1).Username;
        }

        public static string GetValidManagerUsername(HCMContext db)
        {
            return db.Users.First(u => u.RoleId == 2).Username;
        }

        public static string GetValidAdminUsername(HCMContext db)
        {
            return db.Users.First(u => u.RoleId == 3).Username;
        }
    }
}
