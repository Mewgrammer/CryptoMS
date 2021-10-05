using System.Collections.Generic;
using System.Text;
using X509.CSR;
using X509.CSR.Extensions;
using X509.CSR.Models;
using X509.CSR.Models.Constants;
using Xunit;

namespace Tests.CSR;

public class CsrServiceTests
{
    private readonly CsrService _csrService;

    public CsrServiceTests(CsrService csrService)
    {
        _csrService = csrService;
    }
    
    [Fact]
    public void Should_CreateCSR()
    {
        var attributes = new List<CsrAttribute> { new("1.3.6.1.4.1.34380.1.1.3", new AttributeValue(ObjectIdentifiers.Utf8String, "TEST_ATTR"))};
        var csr = _csrService.CreateCsr("TEST_CSR", attributes);
        
        Assert.NotNull(csr);
        var base64 = Encoding.UTF8.GetString(csr.ToBase64());
        Assert.NotEmpty(base64);
        var pemBase64 = csr.ToBase64Pem();
        Assert.NotEmpty(pemBase64);
        Assert.StartsWith("LS0",Encoding.UTF8.GetString(pemBase64));
    }
}