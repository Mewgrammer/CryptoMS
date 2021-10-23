using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data.Entity;

namespace UserManagement.Data;

public class UserDbContext : IdentityDbContext<User, UserRole, Guid>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.UserName)
                .IsUnique();
            entity.ToTable("users");         
        });

        builder.Entity<UserRole>(entity =>
        {
            entity.ToTable("roles");
        });
        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("user_roles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("user_claims");
        });

        builder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("user_logins");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("role_claims");
        });

        builder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("user_tokens");

        });
    }
}