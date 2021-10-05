using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CertificateAuthority.Models;

namespace CertificateAuthority.Data.Entity;

public class CertificateAuthority
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public string Name { get; set; }
    
    public ECaType Type { get; set; }

}