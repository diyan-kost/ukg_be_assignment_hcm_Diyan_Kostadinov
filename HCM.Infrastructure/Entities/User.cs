using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Infrastructure.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Password_Hash { get; set; } = null!;

        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        [Column("employee_id")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
