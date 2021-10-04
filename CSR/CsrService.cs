using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using X509.CSR.Models;
using X509.CSR.Models.Constants;

namespace X509.CSR;

public class CsrService
{
    public Pkcs10CertificationRequest CreateCsr(string commonName, IEnumerable<CsrAttribute> attributes)
    {
        var generator = new Ed25519KeyPairGenerator();
        generator.Init(new KeyGenerationParameters(new SecureRandom(), 1));

        return CreateCsr(commonName, attributes, generator.GenerateKeyPair(), SignatureAlgorithms.Ed25519);
    }
    
    public Pkcs10CertificationRequest CreateCsr(string commonName, IEnumerable<CsrAttribute> attributes,
        AsymmetricCipherKeyPair keyPair, string signatureAlgorithm)
    {
        return new CsrBuilder()
            .SetCommonName(commonName)
            .AddAttribute(attributes.ToArray())
            .Build(signatureAlgorithm, keyPair);
    }
}