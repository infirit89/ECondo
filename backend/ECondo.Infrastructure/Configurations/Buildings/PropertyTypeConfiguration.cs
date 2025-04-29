using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal sealed class PropertyTypeConfiguration : IEntityTypeConfiguration<PropertyType>
{
    public void Configure(EntityTypeBuilder<PropertyType> builder)
    {
        builder.HasKey(pt => pt.Id);
        builder.Property(pt => pt.Name)
            .HasMaxLength(Resources.ShortName)
            .IsRequired();
    }
}