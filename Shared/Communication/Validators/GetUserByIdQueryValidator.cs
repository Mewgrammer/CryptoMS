using Contracts.Communication.Queries;
using FluentValidation;

namespace Contracts.Communication.Validators;

public class GetUserByIdQueryValidator : AbstractValidator<UserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}