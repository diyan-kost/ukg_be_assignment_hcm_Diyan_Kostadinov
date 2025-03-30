using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Infrastructure.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("national_id_number")]
        public string NationalIdNumber { get; set; }

        public string Email { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("current_address")]
        public string CurrentAddress { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        [Column("is_eu_citizen")]
        public bool IsEuCitizen { get; set; }

        [Column("hired_at")]
        public DateTime HiredAt { get; set; }

        [Column("manager_id")]
        public int? ManagerId { get; set; }
        
        public Employee User { get; set; }
    }
}
