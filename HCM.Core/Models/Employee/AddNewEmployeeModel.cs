namespace HCM.Core.Models.Employee
{
    public class AddNewEmployeeModel
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string NationalIdNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public bool IsEuCitizen { get; set; }

        public DateTime HiredAt { get; set; }

        public decimal StartingSalary { get; set; }

        public int? ManagerId { get; set; }
    }
}
