using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationAuthority.Data.Entity;

public class CsrEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string Csr { get; set; }
    
    public string AuthorityKeyIdentifier { get; set; }
}