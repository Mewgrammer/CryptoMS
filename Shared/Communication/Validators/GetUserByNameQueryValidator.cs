using Contracts.Communication.Queries;
using FluentValidation;

namespace Contracts.Communication.Validators;

public class GetUserByNameQueryValidator : AbstractValidator<UserByNameQuery>
{
    public GetUserByNameQueryValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}