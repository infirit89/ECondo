using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name)
            .HasMaxLength(Resources.LongName)
            .IsRequired();
    }
}