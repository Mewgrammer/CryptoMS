using System.Text;
using Org.BouncyCastle.Asn1;
using X509.CSR.Models.Constants;

namespace X509.CSR.Models;

public class AttributeValue
{
    protected string ObjectIdentifier { get; }
    protected object Value { get; }

    public AttributeValue(string objectIdentifier, object value)
    {
        ObjectIdentifier = objectIdentifier;
        Value = value;
    }
    
    private Asn1Encodable? AsAsn1Encodable()
    {
        var valueStr = Value.ToString() ?? string.Empty;
        return ObjectIdentifier switch
        {
            ObjectIdentifiers.Utf8String => new DerUtf8String(valueStr),
            ObjectIdentifiers.OctetString => new DerOctetString(Encoding.UTF8.GetBytes(valueStr)),
            ObjectIdentifiers.BitString => new DerBitString(Encoding.UTF8.GetBytes(valueStr)),
            _ => null
        };
    }
    public virtual DerSet AsDerSet()
    {
        return new DerSet(AsAsn1Encodable());
    }


}