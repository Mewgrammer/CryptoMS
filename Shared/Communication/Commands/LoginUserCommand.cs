using System.ComponentModel.DataAnnotations;
using Contracts.Communication.Contracts;
using MediatR;

namespace Contracts.Communication.Commands;

public class LoginUserCommand : IRequest<UserTokenResponse>
{
    [Required, MinLength(2)] public string UserName { get; set; }
    [Required, MinLength(4)] public string Password { get; set; }
}