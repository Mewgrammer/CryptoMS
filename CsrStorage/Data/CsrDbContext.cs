using Contracts;
using Contracts.Helpers;
using CsrStorage.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CsrStorage.Data;

public class CsrDbContext : DbContext
{
    public DbSet<CsrEntity> CertificateRequests { get; set; }

    public DbSet<CsrEntity> ArchivedCertificateRequests { get; set; }
    
    public CsrDbContext(DbContextOptions<CsrDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CsrEntity>()
            .HasIndex(c => c.CertificateRequest, "idx_csr_csr")
            .IsUnique();
        
        modelBuilder.Entity<CsrEntity>()
            .Property(c => c.CreatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<CsrEntity>()
            .Property(c => c.UpdatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnUpdate();

        modelBuilder.Entity<ArchivedCertificateRequestEntity>()
            .Property(c => c.ArchivedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnAdd();
    }
}