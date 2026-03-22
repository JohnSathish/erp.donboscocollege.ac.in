using ERP.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students", "students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.MobileNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Gender)
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(x => x.Shift)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.AcademicYear)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.ProgramCode)
            .HasMaxLength(32);

        builder.Property(x => x.MajorSubject)
            .HasMaxLength(128);

        builder.Property(x => x.MinorSubject)
            .HasMaxLength(128);

        builder.Property(x => x.AdmissionNumber)
            .HasMaxLength(64);

        builder.Property(x => x.PhotoUrl)
            .HasMaxLength(512);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UserAccountId);

        builder.Property(x => x.UserAccountUsername)
            .HasMaxLength(256);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(x => x.StudentNumber)
            .IsUnique();

        builder.HasIndex(x => x.Email);

        builder.HasIndex(x => x.MobileNumber);

        builder.HasIndex(x => x.ApplicantAccountId)
            .IsUnique();

        builder.HasIndex(x => x.ProgramId);

        builder.HasIndex(x => x.AcademicYear);

        builder.HasIndex(x => x.Status);
    }
}


