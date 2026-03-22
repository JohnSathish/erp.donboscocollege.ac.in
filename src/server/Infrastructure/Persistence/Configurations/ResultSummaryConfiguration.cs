using ERP.Domain.Examinations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class ResultSummaryConfiguration : IEntityTypeConfiguration<ResultSummary>
{
    public void Configure(EntityTypeBuilder<ResultSummary> builder)
    {
        builder.ToTable("ResultSummaries", "examinations");

        builder.HasKey(rs => rs.Id);

        builder.Property(rs => rs.Id)
            .IsRequired();

        builder.Property(rs => rs.StudentId)
            .IsRequired();

        builder.Property(rs => rs.AcademicTermId)
            .IsRequired();

        builder.Property(rs => rs.TotalMarks)
            .IsRequired()
            .HasPrecision(8, 2);

        builder.Property(rs => rs.MaxMarks)
            .IsRequired()
            .HasPrecision(8, 2);

        builder.Property(rs => rs.Percentage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(rs => rs.Grade)
            .HasMaxLength(10);

        builder.Property(rs => rs.GPA)
            .HasPrecision(4, 2);

        builder.Property(rs => rs.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(rs => rs.TotalCredits)
            .IsRequired();

        builder.Property(rs => rs.EarnedCredits)
            .IsRequired();

        builder.Property(rs => rs.CalculatedOnUtc);

        builder.Property(rs => rs.CalculatedBy)
            .HasMaxLength(200);

        builder.Property(rs => rs.PublishedOnUtc);

        builder.Property(rs => rs.PublishedBy)
            .HasMaxLength(200);

        builder.Property(rs => rs.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(rs => rs.CreatedOnUtc)
            .IsRequired();

        builder.Property(rs => rs.CreatedBy)
            .HasMaxLength(200);

        builder.Property(rs => rs.UpdatedOnUtc);

        builder.Property(rs => rs.UpdatedBy)
            .HasMaxLength(200);

        builder.HasIndex(rs => new { rs.StudentId, rs.AcademicTermId })
            .IsUnique();

        builder.HasIndex(rs => rs.StudentId);
        builder.HasIndex(rs => rs.AcademicTermId);
        builder.HasIndex(rs => rs.Status);
    }
}

public class CourseResultConfiguration : IEntityTypeConfiguration<CourseResult>
{
    public void Configure(EntityTypeBuilder<CourseResult> builder)
    {
        builder.ToTable("CourseResults", "examinations");

        builder.HasKey(cr => cr.Id);

        builder.Property(cr => cr.Id)
            .IsRequired();

        builder.Property(cr => cr.ResultSummaryId)
            .IsRequired();

        builder.Property(cr => cr.CourseId)
            .IsRequired();

        builder.Property(cr => cr.AssessmentId);

        builder.Property(cr => cr.TotalMarks)
            .IsRequired()
            .HasPrecision(6, 2);

        builder.Property(cr => cr.MaxMarks)
            .IsRequired()
            .HasPrecision(6, 2);

        builder.Property(cr => cr.Percentage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(cr => cr.Grade)
            .HasMaxLength(10);

        builder.Property(cr => cr.GradePoints)
            .HasPrecision(4, 2);

        builder.Property(cr => cr.Credits)
            .IsRequired();

        builder.Property(cr => cr.IsPassed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(cr => cr.CreatedOnUtc)
            .IsRequired();

        builder.Property(cr => cr.CreatedBy)
            .HasMaxLength(200);

        builder.Property(cr => cr.UpdatedOnUtc);

        builder.Property(cr => cr.UpdatedBy)
            .HasMaxLength(200);

        builder.HasOne(cr => cr.ResultSummary)
            .WithMany(rs => rs.CourseResults)
            .HasForeignKey(cr => cr.ResultSummaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cr => cr.ResultSummaryId);
        builder.HasIndex(cr => cr.CourseId);
        builder.HasIndex(cr => new { cr.ResultSummaryId, cr.CourseId });
    }
}





