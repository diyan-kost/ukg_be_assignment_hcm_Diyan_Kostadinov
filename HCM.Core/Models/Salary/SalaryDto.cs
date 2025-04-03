namespace HCM.Core.Models.Salary
{
    public class SalaryDto
    {
        public decimal Amount { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string? Note { get; set; }
    }
}
