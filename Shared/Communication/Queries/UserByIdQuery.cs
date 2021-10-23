using System.ComponentModel.DataAnnotations;
using Contracts.Communication.Contracts;
using MediatR;

namespace Contracts.Communication.Queries;

public class UserByIdQuery : IRequest<UserResponse>
{
    [Required] public Guid Id { get; set; }
}