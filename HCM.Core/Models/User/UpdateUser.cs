namespace HCM.Core.Models.User
{
    public class UpdateUser
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string? Password { get; set; }

        public int? Role { get; set; }

        public int EmployeeId { get; set; }
    }
}
