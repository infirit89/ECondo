using ECondo.Domain.Payments;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Payments;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.HasOne(p => p.Bill)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BillId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(p => p.PaidByUser)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.PaidByUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Property)
            .WithMany(p => p.Payments)
            .HasForeignKey(p => p.PropertyId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        
        builder.Property(p => p.PaymentMethod)
            .HasMaxLength(Resources.LongName)
            .IsRequired();
    }
}