using Org.BouncyCastle.Asn1;
using BcAttribute = Org.BouncyCastle.Asn1.Cms.Attribute;

namespace X509.CSR.Models;

public class CsrAttribute
{
    public string ObjectIdentifier { get; }
    public AttributeValue Value { get; }

    public CsrAttribute(string objectIdentifier, AttributeValue value)
    {
        ObjectIdentifier = objectIdentifier;
        Value = value;
    }
    
    public CsrAttribute(string objectIdentifier, KeyValuePair<string, string> kvp)
    {
        ObjectIdentifier = objectIdentifier;
        Value = new AttributeValue(kvp.Key, kvp.Value);
    }
    
    public BcAttribute GetEncoded()
    {
        var oid = new DerObjectIdentifier(ObjectIdentifier);
        return new BcAttribute(oid, Value.AsDerSet());
    }
}