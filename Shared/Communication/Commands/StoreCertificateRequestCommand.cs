using System.ComponentModel.DataAnnotations;
using Contracts.Communication.Contracts;
using MediatR;

namespace Contracts.Communication.Commands;

public class StoreCsrCommand : IRequest<CsrResponse>
{
    [Required, MinLength(8)] public string Csr { get; set; }
}