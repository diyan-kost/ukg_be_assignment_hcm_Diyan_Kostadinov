namespace HCM.Core.Models.Employee
{
    public class EmployeeBasicInfo
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HiredAt { get; set; }

        public string? Manager {  get; set; }
    }
}
