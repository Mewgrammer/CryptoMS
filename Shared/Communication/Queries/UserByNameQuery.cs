using System.ComponentModel.DataAnnotations;
using Contracts.Communication.Contracts;
using MediatR;

namespace Contracts.Communication.Queries;

public class UserByNameQuery : IRequest<UserResponse>
{
    [Required, MinLength(2)] public string Name { get; set; }
}