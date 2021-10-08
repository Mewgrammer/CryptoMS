using Contracts;
using CsrStorage.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CsrStorage.Data;

public class CsrDbContext : DbContext
{
    public DbSet<CertificateRequestEntity> CertificateRequests { get; set; }

    public DbSet<CertificateRequestEntity> ArchivedCertificateRequests { get; set; }
    
    public CsrDbContext(DbContextOptions<CsrDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CertificateRequestEntity>()
            .HasIndex(c => c.CertificateRequest, "idx_csr_csr")
            .IsUnique();
        
        modelBuilder.Entity<CertificateRequestEntity>()
            .Property(c => c.CreatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<CertificateRequestEntity>()
            .Property(c => c.UpdatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnUpdate();

        modelBuilder.Entity<ArchivedCertificateRequestEntity>()
            .Property(c => c.ArchivedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnAdd();
    }
}