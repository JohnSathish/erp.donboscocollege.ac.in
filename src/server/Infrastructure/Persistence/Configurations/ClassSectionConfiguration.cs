using ERP.Domain.Academics.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class ClassSectionConfiguration : IEntityTypeConfiguration<ClassSection>
{
    public void Configure(EntityTypeBuilder<ClassSection> builder)
    {
        builder.ToTable("ClassSections", "academics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SectionName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.TermId);
        builder.Property(x => x.TeacherId);
        builder.Property(x => x.TeacherName)
            .HasMaxLength(256);

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.Property(x => x.EnrolledCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.RoomNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Building)
            .HasMaxLength(100);

        builder.Property(x => x.AcademicYear)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Shift)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedOnUtc);
        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => new { x.CourseId, x.AcademicYear, x.Shift });
        builder.HasIndex(x => x.TeacherId);
    }
}

