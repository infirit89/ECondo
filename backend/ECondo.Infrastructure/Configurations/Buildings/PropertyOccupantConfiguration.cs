using ECondo.Domain.Buildings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal sealed class PropertyOccupantConfiguration : IEntityTypeConfiguration<PropertyOccupant>
{
    public void Configure(
        EntityTypeBuilder<PropertyOccupant> builder)
    {
        builder.HasKey(pu => pu.Id);

        builder.HasOne(pu => pu.User)
            .WithMany(u => u.PropertyOccupants)
            .HasForeignKey(pu => pu.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pu => pu.Property)
            .WithMany(p => p.PropertyOccupants)
            .HasForeignKey(pu => pu.PropertyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pu => pu.OccupantType)
            .WithMany(ot => ot.PropertyOccupants)
            .HasForeignKey(pu => pu.OccupantTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(pu => pu.InvitationToken);

        builder.HasIndex(pu => pu.PropertyId);
        builder.HasIndex(pu => pu.UserId);
    }
}