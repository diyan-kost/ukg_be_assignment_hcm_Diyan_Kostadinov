using FluentValidation;
using HCM.Core.Models.User;

namespace HCM.Core.Validators
{
    public class LoginUserModelValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserModelValidator()
        {
            RuleFor(u => u.Username)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .Must(IsValidLoginUsername);

            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1);
        }

        private bool IsValidLoginUsername(string username)
        {
            if (username.Contains(" "))
                return false;

            return true;
        }
    }
}
