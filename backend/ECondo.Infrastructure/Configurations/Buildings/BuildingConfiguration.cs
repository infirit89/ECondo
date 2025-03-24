using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal class BuildingConfiguration : IEntityTypeConfiguration<Building>
{
    public void Configure(EntityTypeBuilder<Building> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name)
            .HasMaxLength(Resources.LongName)
            .IsRequired();

        builder.HasOne(b => b.City)
            .WithMany(c => c.Buildings)
            .HasForeignKey(b => b.CityId)
            .IsRequired();


        builder.Property(b => b.Neighborhood)
            .HasMaxLength(Resources.LongName)
            .IsRequired();

        builder.Property(b => b.PostalCode)
            .HasMaxLength(Resources.ShortName)
            .IsRequired();

        builder.Property(b => b.Street)
            .HasMaxLength(Resources.LongName)
            .IsRequired();

        builder.Property(b => b.StreetNumber)
            .HasMaxLength(Resources.ShortName)
            .IsRequired();

        builder.Property(b => b.BuildingNumber)
            .HasMaxLength(Resources.ShortName)
            .IsRequired();
    }
}