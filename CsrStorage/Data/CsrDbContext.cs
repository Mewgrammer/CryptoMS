using CsrStorage.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;

namespace CsrStorage.Data;

public class CsrDbContext : DbContext
{
    public DbSet<CsrEntity> Csrs { get; set; }

    public DbSet<ArchivedCsrEntity> ArchivedCsrs { get; set; }
    
    public CsrDbContext(DbContextOptions<CsrDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CsrEntity>()
            .HasIndex(c => c.Csr, "idx_csr_csr")
            .IsUnique();
        
        modelBuilder.Entity<CsrEntity>()
            .Property(c => c.CreatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<CsrEntity>()
            .Property(c => c.UpdatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnUpdate();

        modelBuilder.Entity<ArchivedCsrEntity>()
            .Property(c => c.ArchivedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnAdd();
    }
}