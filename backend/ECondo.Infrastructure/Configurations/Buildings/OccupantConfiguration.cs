using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal class OccupantConfiguration : IEntityTypeConfiguration<Occupant>
{
    public void Configure(EntityTypeBuilder<Occupant> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasOne(o => o.Property)
            .WithMany(p => p.Occupants)
            .HasForeignKey(o => o.PropertyId)
            .IsRequired();

        builder.HasOne(o => o.OccupantType)
            .WithMany(ot => ot.Occupants)
            .HasForeignKey(o => o.OccupantTypeId)
            .IsRequired();

        builder.Property(o => o.FirstName)
            .HasMaxLength(Resources.LongName)
            .IsRequired();

        builder.Property(o => o.MiddleName)
            .HasMaxLength(Resources.LongName);

        builder.Property(o => o.LastName)
            .HasMaxLength(Resources.LongName)
            .IsRequired();

        builder.Property(o => o.Email)
            .HasMaxLength(Resources.LongName);
    }
}
