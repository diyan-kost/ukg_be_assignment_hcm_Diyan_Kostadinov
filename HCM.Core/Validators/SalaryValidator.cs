using FluentValidation;
using HCM.Core.Models.Salary;

namespace HCM.Core.Validators
{
    public class SalaryValidator : AbstractValidator<AddSalaryDto>
    {
        public SalaryValidator()
        {
            RuleFor(s => s.Amount)
                .Must(s => s > 1);

            RuleFor(s => s.EffectiveDate)
                .GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}
