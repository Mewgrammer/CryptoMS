using X509.CSR.Models.Enum;

namespace X509.CSR.Models.DTO;

public class CertificateRequestDto
{
    public ECsrFormat Format { get; set; }
    public string Csr { get; set; }
}