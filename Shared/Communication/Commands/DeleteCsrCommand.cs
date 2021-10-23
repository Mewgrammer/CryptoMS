using System.ComponentModel.DataAnnotations;
using Contracts.Communication.Contracts;
using MediatR;

namespace Contracts.Communication.Commands;

public class DeleteCsrCommand : IRequest
{
    [Required, MinLength(8)] public Guid Id { get; set; }
}