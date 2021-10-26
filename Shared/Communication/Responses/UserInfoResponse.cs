namespace Shared.Communication.Responses;

public record UserInfoResponse(string Name, IEnumerable<string> Roles, DateTime ValidUntil);