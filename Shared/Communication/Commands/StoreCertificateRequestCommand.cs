using System.ComponentModel.DataAnnotations;
using MediatR;
using Shared.Communication.Responses;

namespace Shared.Communication.Commands;

public class StoreCsrCommand : IRequest<CsrResponse>
{
    [Required, MinLength(8)] public string Csr { get; set; }
}