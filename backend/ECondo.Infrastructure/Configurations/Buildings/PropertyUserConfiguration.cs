using ECondo.Domain.Buildings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal sealed class PropertyUserConfiguration : IEntityTypeConfiguration<PropertyUser>
{
    public void Configure(EntityTypeBuilder<PropertyUser> builder)
    {
        builder.HasKey(pu => new { pu.PropertyId, pu.UserId, pu.OccupantTypeId });

        builder.HasOne(pu => pu.User)
            .WithMany(u => u.PropertyUsers)
            .HasForeignKey(pu => pu.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pu => pu.Property)
            .WithMany(p => p.PropertyUsers)
            .HasForeignKey(pu => pu.PropertyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pu => pu.OccupantType)
            .WithMany(ot => ot.PropertyUsers)
            .HasForeignKey(pu => pu.OccupantTypeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}