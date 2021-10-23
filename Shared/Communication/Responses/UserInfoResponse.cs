namespace Contracts.Communication.Contracts;

public record UserInfoResponse(string Name, IEnumerable<string> Roles, DateTime ValidUntil);