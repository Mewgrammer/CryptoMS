using System.ComponentModel.DataAnnotations;
using MediatR;
using Shared.Communication.Responses;

namespace Shared.Communication.Queries;

public class CsrByIdQuery : IRequest<CsrResponse>
{
    [Required] public Guid Id { get; set; }
}