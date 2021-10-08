using System.ComponentModel.DataAnnotations.Schema;

public class ArchivedCertificateRequestEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ArchivedAt { get; set; }
    public string CertificateRequest { get; set; }
}