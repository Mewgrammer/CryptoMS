using FluentValidation;
using Shared.Communication.Queries;

namespace Shared.Communication.Validators;

public class GetUserByIdQueryValidator : AbstractValidator<UserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}