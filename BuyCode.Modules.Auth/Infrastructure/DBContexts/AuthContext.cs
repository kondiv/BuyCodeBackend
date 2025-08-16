using BuyCode.Modules.Auth.Domain.Models;
using BuyCodeBackend.Auth.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuyCode.Modules.Auth.Infrastructure.DBContexts;

internal class AuthContext : IdentityDbContext<User, Role, Guid>
{
    public AuthContext()
    {
        
    }

    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
        
    }
    
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("auth");
        
        builder.HasPostgresExtension("uuid-ossp");
        
        base.OnModelCreating(builder);

        builder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
        });

        builder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermissions");

            entity.HasKey(e => new { e.PermissionId, e.RoleId });
            
            entity.HasOne(e => e.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}