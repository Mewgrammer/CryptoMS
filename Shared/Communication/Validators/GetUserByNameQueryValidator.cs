using FluentValidation;
using Shared.Communication.Queries;

namespace Shared.Communication.Validators;

public class GetUserByNameQueryValidator : AbstractValidator<UserByNameQuery>
{
    public GetUserByNameQueryValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}