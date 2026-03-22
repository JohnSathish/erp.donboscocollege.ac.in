using ERP.Domain.Examinations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.ToTable("Assessments", "examinations");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.CourseId)
            .IsRequired();

        builder.Property(a => a.AcademicTermId)
            .IsRequired();

        builder.Property(a => a.ClassSectionId);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.ScheduledDate);

        builder.Property(a => a.Duration)
            .HasConversion(
                v => v.HasValue ? (long?)v.Value.Ticks : null,
                v => v.HasValue ? TimeSpan.FromTicks(v.Value) : null);

        builder.Property(a => a.TotalWeightage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(a => a.MaxMarks)
            .IsRequired();

        builder.Property(a => a.PassingMarks)
            .IsRequired();

        builder.Property(a => a.Instructions)
            .HasMaxLength(2000);

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.PublishedOnUtc);

        builder.Property(a => a.PublishedBy)
            .HasMaxLength(200);

        builder.Property(a => a.CreatedOnUtc)
            .IsRequired();

        builder.Property(a => a.CreatedBy)
            .HasMaxLength(200);

        builder.Property(a => a.UpdatedOnUtc);

        builder.Property(a => a.UpdatedBy)
            .HasMaxLength(200);

        builder.HasIndex(a => new { a.CourseId, a.AcademicTermId, a.Code })
            .IsUnique();

        builder.HasIndex(a => a.AcademicTermId);
        builder.HasIndex(a => a.ClassSectionId);
        builder.HasIndex(a => a.Status);
    }
}





