using FluentValidation;
using HCM.Core.Helpers;
using HCM.Core.Models.User;

namespace HCM.Core.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.Username)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .Must(IsValidUsername);

            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .Must(LoginHelper.IsValidPassword);

            RuleFor(u => u.Role)
                .NotNull()
                .NotEmpty();
        }

        private bool IsValidUsername(string username)
        {
            if (username.Contains(" "))
                return false;

            if (username.All(c => Char.IsDigit(c)))
                return false;

            return true;
        }
    }
}
