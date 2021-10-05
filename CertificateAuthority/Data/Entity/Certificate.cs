using System.ComponentModel.DataAnnotations.Schema;

namespace CertificateAuthority.Data.Entity;

public class Certificate
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public CertificateAuthority Ca { get; set; }
}