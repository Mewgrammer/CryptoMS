namespace X509.CSR.Models;

public class Csr
{
    public Subject Subject { get; set; } = new();
    public List<CsrAttribute> Attributes { get; set; } = new();
}