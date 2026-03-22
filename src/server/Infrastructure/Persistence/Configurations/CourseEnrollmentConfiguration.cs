using ERP.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class CourseEnrollmentConfiguration : IEntityTypeConfiguration<CourseEnrollment>
{
    public void Configure(EntityTypeBuilder<CourseEnrollment> builder)
    {
        builder.ToTable("CourseEnrollments", "students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.TermId);

        builder.Property(x => x.AcademicRecordId);

        builder.Property(x => x.EnrollmentType)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.EnrolledOnUtc)
            .IsRequired();

        builder.Property(x => x.Grade)
            .HasMaxLength(16);

        builder.Property(x => x.MarksObtained)
            .HasColumnType("numeric(6,2)");

        builder.Property(x => x.MaxMarks)
            .HasColumnType("numeric(6,2)");

        builder.Property(x => x.ResultStatus)
            .HasMaxLength(64);

        builder.Property(x => x.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CompletedOnUtc);

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
        builder.HasIndex(x => x.CourseId);
        builder.HasIndex(x => new { x.StudentId, x.CourseId, x.TermId });
        builder.HasIndex(x => x.AcademicRecordId);
    }
}

