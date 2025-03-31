namespace HCM.Core.Models.Salary
{
    public class SalaryDetails
    {
        public decimal Amount { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string? Note { get; set; }
    }
}
