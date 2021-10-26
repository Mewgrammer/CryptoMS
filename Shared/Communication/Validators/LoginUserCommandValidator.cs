using FluentValidation;
using Shared.Communication.Commands;

namespace Shared.Communication.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty().MinimumLength(4);
        RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(4);
    }
}