using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManagement.Data.Entity;
using UserManagement.Helpers;
using UserManagement.Models.Configuration;

namespace UserManagement.Data;

public sealed class UserDbContext : DbContext
{
    private readonly IOptions<DefaultAdminConfiguration> _adminConfig;
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options, IOptions<DefaultAdminConfiguration> adminConfig)
        : base(options)
    {
        _adminConfig = adminConfig;
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(c => c.Name, "idx_user_name")
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasKey(c => c.Id);

        modelBuilder
            .Entity<User>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<User>()
            .Property(c => c.CreatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<User>()
            .Property(c => c.UpdatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnUpdate();

        modelBuilder.Entity<User>()
            .HasMany(e => e.Roles)
            .WithMany(r => r.Users);
        
        modelBuilder.Entity<User>()
            .HasData(new User { Id = Guid.NewGuid(), Name = _adminConfig.Value.Name, Password = new PasswordHasher().Hash(_adminConfig.Value.Password)} );


        modelBuilder.Entity<UserRole>()
            .HasKey(r => r.Name);
        
        modelBuilder.Entity<UserRole>()
            .Property(r => r.CreatedAt)
            .HasValueGenerator<UtcDateValueGenerator>()
            .ValueGeneratedOnAdd();

    }
}