namespace Contracts.CsrStorage;

public class CertificateRequestDto
{
    public Guid Id { get; set; }
    public string CertificateRequest { get; set; }
    public DateTime CreatedAt { get; set; }
}