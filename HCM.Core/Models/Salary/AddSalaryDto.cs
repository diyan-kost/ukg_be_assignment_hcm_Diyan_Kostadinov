namespace HCM.Core.Models.Salary
{
    public class AddSalaryDto
    {
        public decimal Amount { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int EmployeeId { get; set; }

        public string? Note { get; set; }
    }
}
