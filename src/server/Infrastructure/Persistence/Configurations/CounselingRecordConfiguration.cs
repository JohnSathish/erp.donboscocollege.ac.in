using ERP.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class CounselingRecordConfiguration : IEntityTypeConfiguration<CounselingRecord>
{
    public void Configure(EntityTypeBuilder<CounselingRecord> builder)
    {
        builder.ToTable("CounselingRecords", "students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.SessionType)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.CounselorName)
            .HasMaxLength(256);

        builder.Property(x => x.CounselorId)
            .HasMaxLength(128);

        builder.Property(x => x.Location)
            .HasMaxLength(256);

        builder.Property(x => x.Issues)
            .HasMaxLength(2000);

        builder.Property(x => x.Discussion)
            .HasMaxLength(5000);

        builder.Property(x => x.Recommendations)
            .HasMaxLength(2000);

        builder.Property(x => x.ActionPlan)
            .HasMaxLength(2000);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.SessionDate);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.IsFollowUpRequired);
    }
}

