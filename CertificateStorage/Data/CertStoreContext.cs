using Microsoft.EntityFrameworkCore;

namespace CertificateStorage.Data;

public class CertStoreContext : DbContext
{
    public DbSet<Entity.CertificateEntity> Certificates { get; set; }
}