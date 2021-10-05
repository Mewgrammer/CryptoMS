using Microsoft.EntityFrameworkCore;
using RegistrationAuthority.Data.Entity;

namespace RegistrationAuthority.Data;

public class RaContext : DbContext
{
    public DbSet<CsrEntity> CertificateRequests { get; set; }
    
}