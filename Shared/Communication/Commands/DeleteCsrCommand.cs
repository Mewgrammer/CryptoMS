using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Shared.Communication.Commands;

public class DeleteCsrCommand : IRequest
{
    [Required, MinLength(8)] public Guid Id { get; set; }
}