using ERP.Domain.Fees.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class ScholarshipConfiguration : IEntityTypeConfiguration<Scholarship>
{
    public void Configure(EntityTypeBuilder<Scholarship> builder)
    {
        builder.ToTable("Scholarships", "fees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.ScholarshipName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Percentage)
            .HasColumnType("numeric(5,2)");

        builder.Property(x => x.FixedAmount)
            .HasColumnType("numeric(18,2)");

        builder.Property(x => x.AcademicYear)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.SponsorName)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovalReference)
            .HasMaxLength(128);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.AcademicYear);
        builder.HasIndex(x => x.IsActive);
    }
}

