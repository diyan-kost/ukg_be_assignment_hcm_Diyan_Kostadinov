using FluentValidation;
using HCM.Core.Models.Employee;

namespace HCM.Core.Validators
{
    public class AddNewEmployeeValidator : AbstractValidator<AddNewEmployeeModel>
    {
        public AddNewEmployeeValidator()
        {
            RuleFor(e => e.FirstName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(255);

            RuleFor(e => e.MiddleName)
                            .MinimumLength(1).When(e => !string.IsNullOrEmpty(e.MiddleName))
                            .MaximumLength(255).When(e => !string.IsNullOrEmpty(e.MiddleName));

            RuleFor(e => e.LastName)
                .MinimumLength(1).When(e => !string.IsNullOrEmpty(e.LastName))
                .MaximumLength(255).When(e => !string.IsNullOrEmpty(e.LastName));

            RuleFor(e => e.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .Must(IsValidPhoneNumber)
                .WithMessage("Phone number is not in correct format!");

            RuleFor(e => e.Email)
                .EmailAddress();

            RuleFor(e => e.Address)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(255);

            RuleFor(e => e.NationalIdNumber)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length < 10)
                return false;

            for (var i = 0; i < phoneNumber.Length; i++)
            {
                if (i == 0 && phoneNumber[i] != '+' && !Char.IsDigit(phoneNumber[i]))
                    return false;

                if (i != 0 && !Char.IsDigit(phoneNumber[i]))
                    return false;
            }

            return true;
        }
    }
}
