using ECondo.Domain.Users;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Identity;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder
            .HasIndex(u => u.NormalizedUserName)
            .HasDatabaseName("UserNameIndex")
            .IsUnique();
        builder
            .HasIndex(u => u.NormalizedEmail)
            .HasDatabaseName("EmailIndex");
        builder
            .Property(u => u.ConcurrencyStamp)
            .IsConcurrencyToken();

        builder
            .Property(u => u.UserName)
            .HasMaxLength(Resources.LongName);
        builder
            .Property(u => u.NormalizedUserName)
            .HasMaxLength(Resources.LongName);
        builder
            .Property(u => u.Email)
            .HasMaxLength(Resources.LongName);
        builder
            .Property(u => u.NormalizedEmail)
            .HasMaxLength(Resources.LongName);
        builder
            .Property(u => u.PhoneNumber)
            .HasMaxLength(Resources.LongName);

        builder
            .HasQueryFilter(u => !u.IsDeleted);

        builder
            .HasIndex(u => u.IsDeleted)
            .HasFilter("IsDeleted = 0");

        builder.ToTable("Users");
    }
}
