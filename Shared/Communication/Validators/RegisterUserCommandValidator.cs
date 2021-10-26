using FluentValidation;
using Shared.Communication.Commands;

namespace Shared.Communication.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty().MinimumLength(4);
        RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(4);
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
    }
}