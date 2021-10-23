using System.ComponentModel.DataAnnotations;
using Contracts.Communication.Contracts;
using MediatR;

namespace Contracts.Communication.Queries;

public class CsrByIdQuery : IRequest<CsrResponse>
{
    [Required] public Guid Id { get; set; }
}