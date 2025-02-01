using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Infrastructure.Contexts;

internal class ECondoDbContext(DbContextOptions<ECondoDbContext> options) : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    public override DbSet<User> Users { get; set; } = null!;
    public override DbSet<Role> Roles { get; set; } = null!;
    public override DbSet<UserRole> UserRoles { get; set; } = null!;
    public override DbSet<UserClaim> UserClaims { get; set; } = null!;
    public override DbSet<UserLogin> UserLogins { get; set; } = null!;
    public override DbSet<UserToken> UserTokens { get; set; } = null!;
    public override DbSet<RoleClaim> RoleClaims { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
            builder.Property(u => u.PhoneNumber).HasMaxLength(256);

            builder.HasQueryFilter(u => !u.IsDeleted);
            builder.HasIndex(u => u.IsDeleted).HasFilter("IsDeleted = 0");

            builder.ToTable("Users");
        });

        modelBuilder.Entity<Role>(builder =>
        {
            builder.HasKey(r => r.Id);
            builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            builder.Property(r => r.Name).HasMaxLength(256);
            builder.Property(r => r.NormalizedName).HasMaxLength(256);

            builder.ToTable("Roles");
        });

        modelBuilder.Entity<UserRole>(builder =>
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
            builder.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
            builder.ToTable("UserRoles");
        });

        modelBuilder.Entity<UserClaim>(builder =>
        {
            builder.HasKey(uc => uc.Id);
            builder.HasOne(uc => uc.User).WithMany(u => u.UserClaims).HasForeignKey(uc => uc.UserId).IsRequired();

            builder.ToTable("UserClaims");
        });

        modelBuilder.Entity<RoleClaim>(builder =>
        {
            builder.HasKey(rc => rc.Id);
            builder.HasOne(rc => rc.Role).WithMany(r => r.RoleClaims).HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.ToTable("RoleClaims");
        });

        modelBuilder.Entity<UserLogin>(builder =>
        {
            builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            builder.Property(l => l.LoginProvider).HasMaxLength(128);
            builder.Property(l => l.ProviderKey).HasMaxLength(128);
            builder.HasOne(ul => ul.User).WithMany(u => u.UserLogins).HasForeignKey(ul => ul.UserId).IsRequired();

            builder.ToTable("UserLogins");
        });

        modelBuilder.Entity<UserToken>(builder =>
        {
            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            builder.Property(t => t.LoginProvider).HasMaxLength(128);
            builder.Property(t => t.Name).HasMaxLength(128);

            builder.HasOne(ut => ut.User).WithMany(u => u.UserTokens).HasForeignKey(ut => ut.UserId).IsRequired();

            builder.ToTable("UserTokens");
        });
    }
}
