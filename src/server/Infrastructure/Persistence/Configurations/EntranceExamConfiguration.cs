using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class EntranceExamConfiguration : IEntityTypeConfiguration<EntranceExam>
{
    public void Configure(EntityTypeBuilder<EntranceExam> builder)
    {
        builder.ToTable("EntranceExams", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExamName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.ExamCode)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.ExamDate)
            .IsRequired();

        builder.Property(x => x.ExamStartTime)
            .IsRequired()
            .HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));

        builder.Property(x => x.ExamEndTime)
            .IsRequired()
            .HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));

        builder.Property(x => x.Venue)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.VenueAddress)
            .HasMaxLength(500);

        builder.Property(x => x.Instructions)
            .HasMaxLength(2000);

        builder.Property(x => x.MaxCapacity)
            .IsRequired();

        builder.Property(x => x.CurrentRegistrations)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedOnUtc);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.ExamCode)
            .IsUnique();

        builder.HasIndex(x => x.ExamDate);

        builder.HasIndex(x => x.IsActive);

        builder.HasMany(x => x.Registrations)
            .WithOne(x => x.Exam)
            .HasForeignKey(x => x.ExamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}













