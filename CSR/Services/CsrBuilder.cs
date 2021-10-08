using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using X509.CSR.Models;

namespace X509.CSR.Services;

public class CsrBuilder
{
    private readonly Csr _csr = new();

    public CsrBuilder SetSubject(Subject subject)
    {
        _csr.Subject = subject;
        return this;
    }

    public CsrBuilder SetCommonName(string cn)
    {
        _csr.Subject.CommonName = cn;
        return this;
    }


    public CsrBuilder AddAttribute(params CsrAttribute[] attributes)
    {
        _csr.Attributes.AddRange(attributes);
        return this;
    }

    public Pkcs10CertificationRequest Build(string algorithm, AsymmetricCipherKeyPair keyPair)
    {
        var attributes = Asn1EncodableVector.FromEnumerable(_csr.Attributes.Select(a => a.GetEncoded()));
        return new Pkcs10CertificationRequest(algorithm, _csr.Subject.AsX509Name, keyPair.Public,
            new DerSet(attributes), keyPair.Private);
    }
}