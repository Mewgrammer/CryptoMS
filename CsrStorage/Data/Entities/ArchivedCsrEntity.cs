using System.ComponentModel.DataAnnotations.Schema;

namespace CsrStorage.Data.Entities;

public class ArchivedCsrEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ArchivedAt { get; set; }
    public string CertificateRequest { get; set; }
}