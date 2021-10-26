namespace Shared.Communication.Responses;

public record CsrResponse(Guid Id, string CertificateRequest, DateTime CreatedAt);