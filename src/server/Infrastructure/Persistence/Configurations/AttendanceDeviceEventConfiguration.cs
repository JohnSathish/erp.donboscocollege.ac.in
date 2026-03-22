using ERP.Domain.Attendance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class AttendanceDeviceEventConfiguration : IEntityTypeConfiguration<AttendanceDeviceEvent>
{
    public void Configure(EntityTypeBuilder<AttendanceDeviceEvent> builder)
    {
        builder.ToTable("AttendanceDeviceEvents", "attendance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DeviceId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.DeviceType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PersonId);
        builder.Property(x => x.PersonType)
            .HasConversion<int>();

        builder.Property(x => x.CardNumber)
            .HasMaxLength(100);

        builder.Property(x => x.EventTimestamp)
            .IsRequired();

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(EventType.Scan);

        builder.Property(x => x.IsProcessed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.AttendanceRecordId);
        builder.Property(x => x.ProcessedOnUtc);
        builder.Property(x => x.ProcessingError)
            .HasMaxLength(1000);

        builder.Property(x => x.RawData)
            .HasMaxLength(2000);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.HasIndex(x => new { x.DeviceId, x.EventTimestamp });
        builder.HasIndex(x => new { x.PersonId, x.EventTimestamp });
        builder.HasIndex(x => new { x.CardNumber, x.EventTimestamp });
        builder.HasIndex(x => x.IsProcessed);
    }
}

