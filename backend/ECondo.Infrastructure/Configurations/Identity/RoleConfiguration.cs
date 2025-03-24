using ECondo.Domain.Users;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Identity;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

        builder.Property(r => r.Name).HasMaxLength(Resources.LongName);
        builder.Property(r => r.NormalizedName).HasMaxLength(Resources.LongName);

        builder.ToTable("Roles");
    }
}
