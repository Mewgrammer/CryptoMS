using System.ComponentModel.DataAnnotations;
using MediatR;
using Shared.Communication.Responses;

namespace Shared.Communication.Commands;

public class RegisterUserCommand : IRequest<UserTokenResponse>
{
    [Required, MinLength(2)] public string UserName { get; set; }
    [Required, EmailAddress] public string Email { get; set; }
    [Required, MinLength(4)] public string Password { get; set; }
}