using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class ExamRegistrationConfiguration : IEntityTypeConfiguration<ExamRegistration>
{
    public void Configure(EntityTypeBuilder<ExamRegistration> builder)
    {
        builder.ToTable("ExamRegistrations", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExamId)
            .IsRequired();

        builder.Property(x => x.ApplicantAccountId)
            .IsRequired();

        builder.Property(x => x.HallTicketNumber)
            .HasMaxLength(64);

        builder.Property(x => x.IsPresent)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.Score);

        builder.Property(x => x.RegisteredOnUtc)
            .IsRequired();

        builder.Property(x => x.RegisteredBy)
            .HasMaxLength(256);

        builder.Property(x => x.AttendanceMarkedOnUtc);

        builder.Property(x => x.AttendanceMarkedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ScoreEnteredOnUtc);

        builder.Property(x => x.ScoreEnteredBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.ExamId);

        builder.HasIndex(x => x.ApplicantAccountId);

        builder.HasIndex(x => x.HallTicketNumber)
            .IsUnique()
            .HasFilter("\"HallTicketNumber\" IS NOT NULL");

        builder.HasIndex(x => new { x.ExamId, x.ApplicantAccountId })
            .IsUnique();

        builder.HasOne(x => x.Exam)
            .WithMany(x => x.Registrations)
            .HasForeignKey(x => x.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ApplicantAccount)
            .WithMany()
            .HasForeignKey(x => x.ApplicantAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

