using System.Text;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Utilities.Encoders;

namespace X509.CSR.Extensions;

public static class CsrExtensions
{
    public static byte[] ToBase64(this Pkcs10CertificationRequest csr)
    {
        return Base64.Encode(csr.GetCertificationRequestInfo().GetDerEncoded());
    }
    
    public static byte[] ToBase64Pem(this Pkcs10CertificationRequest csr)
    {
        return Base64.Encode(Encoding.UTF8.GetBytes(ToPemString(csr)));
    }
    
    public static string ToPemString(this Pkcs10CertificationRequest csr)
    {
        return $"-----BEGIN CERTIFICATE REQUEST-----\n{Encoding.UTF8.GetString(ToBase64(csr))}\n-----END CERTIFICATE REQUEST-----";
    }

}