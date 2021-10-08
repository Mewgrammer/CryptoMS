using System.ComponentModel.DataAnnotations;

namespace Contracts.CsrStorage;

public class StoreCsrDto
{
    [Required] public string CertificateRequest { get; set; }
}