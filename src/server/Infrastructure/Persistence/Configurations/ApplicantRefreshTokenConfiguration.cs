using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class ApplicantRefreshTokenConfiguration : IEntityTypeConfiguration<ApplicantRefreshToken>
{
    public void Configure(EntityTypeBuilder<ApplicantRefreshToken> builder)
    {
        builder.ToTable("ApplicantRefreshTokens", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TokenHash)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.ExpiresOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.HasIndex(x => x.TokenHash)
            .IsUnique();
    }
}

