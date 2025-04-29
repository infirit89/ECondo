using ECondo.Domain.Profiles;
using ECondo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECondo.Infrastructure.Configurations.Profile;

internal class ProfileDetailsConfiguration : IEntityTypeConfiguration<ProfileDetails>
{
    public void Configure(EntityTypeBuilder<ProfileDetails> builder)
    {
        builder.HasKey(ud => ud.Id);
        builder.Property(ud => ud.FirstName).HasMaxLength(Resources.LongName);
        builder.Property(ud => ud.MiddleName).HasMaxLength(Resources.LongName);
        builder.Property(ud => ud.LastName).HasMaxLength(Resources.LongName);
        builder.HasOne(ud => ud.User).WithMany(u => u.UserDetails).HasForeignKey(ud => ud.UserId);
    }
}