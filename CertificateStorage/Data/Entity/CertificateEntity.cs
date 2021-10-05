using System.ComponentModel.DataAnnotations.Schema;

namespace CertificateStorage.Data.Entity;

public class CertificateEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string Certificate { get; set; }
}