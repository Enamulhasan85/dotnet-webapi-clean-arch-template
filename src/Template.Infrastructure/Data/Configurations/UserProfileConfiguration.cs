using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities;
using Template.Domain.ValueObjects;

namespace Template.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        // Value object conversions (can't be done with attributes)
        builder.Property(x => x.Email)
            .HasConversion(
                email => email != null ? email.Value : null,
                value => value != null ? new Email(value) : null);

        builder.Property(x => x.PhoneNumber)
            .HasConversion(
                phone => phone != null ? phone.Value : null,
                value => value != null ? new PhoneNumber(value) : null);

        // Address as JSON (can't be done with attributes)
        builder.OwnsOne(x => x.Address, address =>
        {
            address.ToJson();
        });

        // Unique constraint (can't be done with attributes)
        builder.HasIndex(x => x.ApplicationUserId).IsUnique();
    }
}
