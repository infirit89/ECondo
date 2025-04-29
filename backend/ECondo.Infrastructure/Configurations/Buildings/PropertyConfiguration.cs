using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Floor)
            .HasMaxLength(Resources.LongName)
            .IsRequired();

        builder.Property(p => p.Number)
            .HasMaxLength(Resources.LongName)
            .IsRequired();

        builder.HasOne(p => p.Entrance)
            .WithMany(e => e.Properties)
            .HasForeignKey(p => p.EntranceId)
            .IsRequired();

        builder.HasOne(p => p.PropertyType)
            .WithMany(pt => pt.Properties)
            .HasForeignKey(p => p.PropertyTypeId)
            .IsRequired();

        builder.HasIndex(p => p.Number);
    }
}
