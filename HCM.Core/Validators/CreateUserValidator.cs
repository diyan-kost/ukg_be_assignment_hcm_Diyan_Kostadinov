using FluentValidation;
using HCM.Core.Models.User;

namespace HCM.Core.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.Username)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1);

            RuleFor(u => u.Role)
                .NotNull()
                .NotEmpty();
        }
    }
}
