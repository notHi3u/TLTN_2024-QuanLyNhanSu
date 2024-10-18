using Account.Domain.Models;
using EMS.Domain.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
namespace Account.Infrastructure.Context
{
    public class EMSDbContext
        : IdentityDbContext<
            User, Role, string,
            IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public EMSDbContext(DbContextOptions<EMSDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(b =>
            {

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            builder.Entity<Role>(b =>
            {
                b.Property(r => r.Description)
                .HasMaxLength(250);
                b.HasMany(e => e.UserRoles)
              .WithOne(e => e.Role)
              .HasForeignKey(ur => ur.RoleId)
              .IsRequired();
            });
            builder.Entity<RolePermission>()
        .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RefreshToken>().HasKey(t => t.Id);

            builder.HasDefaultSchema("Account");

            // Fluent API configurations go here.
            // For example:
            // builder.Entity<SomeEntity>().Property(e => e.PropertyName).IsRequired();
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
    }
}
