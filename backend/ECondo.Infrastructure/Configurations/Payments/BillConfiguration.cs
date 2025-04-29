using ECondo.Domain.Payments;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Payments;

internal sealed class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title)
            .HasMaxLength(Resources.LongName)
            .IsRequired();

        builder.Property(b => b.Description)
            .HasMaxLength(Resources.LongName);

        builder
            .HasOne(b => b.Entrance)
            .WithMany(e => e.Bills)
            .HasForeignKey(b => b.EntranceId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(b => b.CreatedByUser)
            .WithMany(u => u.Bills)
            .HasForeignKey(b => b.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        
        
    }
}