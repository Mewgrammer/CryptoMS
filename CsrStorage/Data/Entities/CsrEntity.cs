using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace CsrStorage.Data.Entities;

public class CsrEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CertificateRequest { get; set; }
}