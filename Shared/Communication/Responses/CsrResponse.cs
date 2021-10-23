namespace Contracts.Communication.Contracts;

public record CsrResponse(Guid Id, string CertificateRequest, DateTime CreatedAt);