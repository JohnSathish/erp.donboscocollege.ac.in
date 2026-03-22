using ERP.Domain.Academics.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class TimetableSlotConfiguration : IEntityTypeConfiguration<TimetableSlot>
{
    public void Configure(EntityTypeBuilder<TimetableSlot> builder)
    {
        builder.ToTable("TimetableSlots", "academics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClassSectionId)
            .IsRequired();

        builder.HasOne(x => x.ClassSection)
            .WithMany()
            .HasForeignKey(x => x.ClassSectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.DayOfWeek)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.StartTime)
            .IsRequired()
            .HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));

        builder.Property(x => x.EndTime)
            .IsRequired()
            .HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));

        builder.Property(x => x.RoomNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Building)
            .HasMaxLength(100);

        builder.Property(x => x.TeacherId);
        builder.Property(x => x.TeacherName)
            .HasMaxLength(256);

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

        builder.HasIndex(x => new { x.ClassSectionId, x.DayOfWeek, x.StartTime });
        builder.HasIndex(x => new { x.TeacherId, x.DayOfWeek, x.StartTime });
        builder.HasIndex(x => new { x.RoomNumber, x.DayOfWeek, x.StartTime });
    }
}

