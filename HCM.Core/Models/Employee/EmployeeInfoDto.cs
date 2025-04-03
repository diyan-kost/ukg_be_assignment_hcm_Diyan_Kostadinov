using System.Text;

namespace HCM.Core.Models.Employee
{
    public class EmployeeInfoDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HiredAt { get; set; }

        public string? ManagerFirstName {  get; set; }
        public string? ManagerMiddleName {  get; set; }
        public string? ManagerLastName {  get; set; }

        public string FullName()
        {
            var fullName = new StringBuilder(FirstName);

            if (!string.IsNullOrEmpty(MiddleName))
                fullName.Append($" {MiddleName}");

            if (!string.IsNullOrEmpty(LastName))
                fullName.Append($" {LastName}");

            return fullName.ToString();
        }

        public string ManagerFullName()
        {
            var fullName = new StringBuilder(ManagerFirstName);

            if (!string.IsNullOrEmpty(ManagerMiddleName))
                fullName.Append($" {ManagerMiddleName}");

            if (!string.IsNullOrEmpty(ManagerLastName))
                fullName.Append($" {ManagerLastName}");

            return fullName.ToString();
        }
    }
}
