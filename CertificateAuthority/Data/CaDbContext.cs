using Microsoft.EntityFrameworkCore;

namespace CertificateAuthority.Data;

public class CaContext : DbContext
{
    public DbSet<Entity.CertificateAuthority> CertificateAuthorities { get; set; }
    
    public DbSet<Entity.Certificate> Certificates { get; set; }
    
    public CaContext(DbContextOptions<CaContext> options)
        : base(options)
    { }

}