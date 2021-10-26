using System.ComponentModel.DataAnnotations;
using MediatR;
using Shared.Communication.Responses;

namespace Shared.Communication.Queries;

public class UserByNameQuery : IRequest<UserResponse>
{
    [Required, MinLength(2)] public string Name { get; set; }
}