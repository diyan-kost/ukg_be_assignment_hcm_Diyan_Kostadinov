using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Infrastructure.Entities
{
    public class Salary
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        [Column("effective_date")]
        public DateTime EffectiveDate { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string? Note { get; set; }
    }
}
