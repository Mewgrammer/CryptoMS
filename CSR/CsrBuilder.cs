using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Pkcs;
using X509.CSR.Models;
using BcAttribute = Org.BouncyCastle.Asn1.Cms.Attribute;

namespace X509.CSR;

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
        var signatureFactory = new Asn1SignatureFactory(algorithm, keyPair.Private);
        var attributes = new Asn1EncodableVector(_csr.Attributes.Select(a => a.GetEncoded()).ToArray());
        return new Pkcs10CertificationRequest(signatureFactory, _csr.Subject.AsX509Name, keyPair.Public, new DerSet(attributes), keyPair.Private);
    }

}