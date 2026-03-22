using ERP.Domain.Attendance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("AttendanceRecords", "attendance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SessionId)
            .IsRequired();

        builder.HasOne(x => x.Session)
            .WithMany()
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.PersonId)
            .IsRequired();

        builder.Property(x => x.PersonType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(AttendanceStatus.Present);

        builder.Property(x => x.MarkedOnUtc)
            .IsRequired();

        builder.Property(x => x.MarkedBy)
            .HasMaxLength(256);

        builder.Property(x => x.DeviceId)
            .HasMaxLength(100);

        builder.Property(x => x.DeviceType)
            .HasMaxLength(50);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedOnUtc);
        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => new { x.SessionId, x.PersonId });
        builder.HasIndex(x => new { x.PersonId, x.PersonType, x.MarkedOnUtc });
    }
}

