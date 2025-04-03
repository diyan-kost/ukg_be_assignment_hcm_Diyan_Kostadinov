namespace HCM.Core.Models.User
{
    public class CreateUserDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public int EmployeeId { get; set; }
    }
}
