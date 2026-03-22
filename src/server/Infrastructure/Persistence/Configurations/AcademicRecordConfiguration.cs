using ERP.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class AcademicRecordConfiguration : IEntityTypeConfiguration<AcademicRecord>
{
    public void Configure(EntityTypeBuilder<AcademicRecord> builder)
    {
        builder.ToTable("AcademicRecords", "students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.TermId);

        builder.Property(x => x.AcademicYear)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Semester)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.GPA)
            .HasColumnType("numeric(4,2)");

        builder.Property(x => x.CGPA)
            .HasColumnType("numeric(4,2)");

        builder.Property(x => x.Grade)
            .HasMaxLength(16);

        builder.Property(x => x.ResultStatus)
            .HasMaxLength(64);

        builder.Property(x => x.TotalCredits)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.CreditsEarned)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedOnUtc);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => new { x.StudentId, x.AcademicYear, x.Semester });
        builder.HasIndex(x => x.TermId);
    }
}

