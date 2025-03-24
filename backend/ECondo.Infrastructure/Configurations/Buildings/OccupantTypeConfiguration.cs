using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal class OccupantTypeConfiguration : IEntityTypeConfiguration<OccupantType>
{
    public void Configure(EntityTypeBuilder<OccupantType> builder)
    {
        builder.HasKey(ot => ot.Id);
        builder.Property(ot => ot.Name)
            .HasMaxLength(Resources.LongName)
            .IsRequired();
    }
}