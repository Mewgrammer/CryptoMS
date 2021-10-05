using Microsoft.EntityFrameworkCore;
using X509.RegistrationAuthority.Data.Entity;

namespace X509.RegistrationAuthority.Data;

public class RaContext : DbContext
{
    public DbSet<CsrEntity> CertificateRequests { get; set; }
    
}