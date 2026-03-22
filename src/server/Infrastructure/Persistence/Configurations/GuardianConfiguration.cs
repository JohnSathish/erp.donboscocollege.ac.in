using ERP.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class GuardianConfiguration : IEntityTypeConfiguration<Guardian>
{
    public void Configure(EntityTypeBuilder<Guardian> builder)
    {
        builder.ToTable("Guardians", "students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Relationship)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.Age)
            .HasMaxLength(16);

        builder.Property(x => x.Occupation)
            .HasMaxLength(256);

        builder.Property(x => x.ContactNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.Property(x => x.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

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

        builder.Property(x => x.UserAccountId);

        builder.Property(x => x.UserAccountUsername)
            .HasMaxLength(256);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => new { x.StudentId, x.Relationship });
    }
}

