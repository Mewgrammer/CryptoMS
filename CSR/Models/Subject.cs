using Org.BouncyCastle.Asn1.X509;

namespace X509.CSR.Models;

public class Subject
{
    public string CommonName { get; set; }
    public X509Name AsX509Name => new (ToString());

    public override string ToString()
    {
        return "CN=" + CommonName;
    }
    
    
}