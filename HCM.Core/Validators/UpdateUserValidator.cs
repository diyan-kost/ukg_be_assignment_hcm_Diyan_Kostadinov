using FluentValidation;
using HCM.Core.Helpers;
using HCM.Core.Models.User;

namespace HCM.Core.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(u => u.Password)
                .MinimumLength(1)
                .Must(LoginHelper.IsValidPassword)
                .When(u => !string.IsNullOrEmpty(u.Password));
        }
    }
}
