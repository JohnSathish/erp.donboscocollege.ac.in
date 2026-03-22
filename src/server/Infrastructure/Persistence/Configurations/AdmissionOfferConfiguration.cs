using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class AdmissionOfferConfiguration : IEntityTypeConfiguration<AdmissionOffer>
{
    public void Configure(EntityTypeBuilder<AdmissionOffer> builder)
    {
        builder.ToTable("AdmissionOffers", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId)
            .IsRequired();

        builder.HasIndex(x => x.AccountId)
            .IsUnique();

        builder.Property(x => x.ApplicationNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.HasIndex(x => x.ApplicationNumber);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.MeritRank)
            .IsRequired();

        builder.Property(x => x.Shift)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.MajorSubject)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasIndex(x => x.Status);

        builder.Property(x => x.OfferDate)
            .IsRequired();

        builder.Property(x => x.ExpiryDate)
            .IsRequired();

        builder.Property(x => x.AcceptedOnUtc);

        builder.Property(x => x.RejectedOnUtc);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1024);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();
    }
}

