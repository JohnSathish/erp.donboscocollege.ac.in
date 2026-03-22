using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
{
    public void Configure(EntityTypeBuilder<Applicant> builder)
    {
        builder.ToTable("Applicants", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ApplicationNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.HasIndex(x => x.ApplicationNumber)
            .IsUnique();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(x => x.Email);

        builder.Property(x => x.MobileNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.ProgramCode)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.HasIndex(x => x.Status);

        builder.Property(x => x.StatusUpdatedOnUtc)
            .IsRequired();

        builder.Property(x => x.StatusUpdatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.StatusRemarks)
            .HasMaxLength(1024);

        builder.Property(x => x.EntranceExamScheduledOnUtc);

        builder.Property(x => x.EntranceExamVenue)
            .HasMaxLength(256);

        builder.Property(x => x.EntranceExamInstructions)
            .HasMaxLength(2048);
    }
}


