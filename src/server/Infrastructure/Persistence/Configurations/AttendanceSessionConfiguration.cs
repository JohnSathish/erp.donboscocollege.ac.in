using ERP.Domain.Attendance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class AttendanceSessionConfiguration : IEntityTypeConfiguration<AttendanceSession>
{
    public void Configure(EntityTypeBuilder<AttendanceSession> builder)
    {
        builder.ToTable("AttendanceSessions", "attendance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SessionName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.ClassSectionId);
        builder.Property(x => x.CourseId);
        builder.Property(x => x.StaffShiftId);
        builder.Property(x => x.SessionDate)
            .IsRequired();

        builder.Property(x => x.StartTime)
            .HasConversion(
                v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,
                v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : null);

        builder.Property(x => x.EndTime)
            .HasConversion(
                v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,
                v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : null);

        builder.Property(x => x.AcademicYear)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Term)
            .HasMaxLength(50);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.IsMarked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.MarkedOnUtc);
        builder.Property(x => x.MarkedBy)
            .HasMaxLength(256);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedOnUtc);
        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => new { x.SessionDate, x.ClassSectionId, x.CourseId });
        builder.HasIndex(x => new { x.AcademicYear, x.Term });
    }
}

