using Contracts.CsrStorage;
using FluentValidation;

namespace Contracts.Validation;

public class CsrValidator : AbstractValidator<StoreCsrDto>
{
    public CsrValidator()
    {
        RuleFor(x => x.CertificateRequest).NotEmpty();
    }
}