using System.ComponentModel.DataAnnotations;
using MediatR;
using Shared.Communication.Responses;

namespace Shared.Communication.Queries;

public class UserByIdQuery : IRequest<UserResponse>
{
    [Required] public Guid Id { get; set; }
}