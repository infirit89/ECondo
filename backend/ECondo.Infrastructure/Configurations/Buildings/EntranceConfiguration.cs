using ECondo.Domain.Buildings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Buildings;

internal class EntranceConfiguration : IEntityTypeConfiguration<Entrance>
{
    public void Configure(EntityTypeBuilder<Entrance> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Building)
            .WithMany(b => b.Entrances)
            .HasForeignKey(e => e.BuildingId)
            .IsRequired();

        builder.HasOne(e => e.Manager)
            .WithMany(u => u.Entrances)
            .HasForeignKey(e => e.ManagerId)
            .IsRequired();
    }
}