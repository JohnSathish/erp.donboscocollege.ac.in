using ERP.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class DisciplineRecordConfiguration : IEntityTypeConfiguration<DisciplineRecord>
{
    public void Configure(EntityTypeBuilder<DisciplineRecord> builder)
    {
        builder.ToTable("DisciplineRecords", "students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.IncidentType)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.Location)
            .HasMaxLength(256);

        builder.Property(x => x.ReportedBy)
            .HasMaxLength(256);

        builder.Property(x => x.Witnesses)
            .HasMaxLength(500);

        builder.Property(x => x.Severity)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ActionTaken)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ActionDetails)
            .HasMaxLength(2000);

        builder.Property(x => x.ActionTakenBy)
            .HasMaxLength(256);

        builder.Property(x => x.ResolutionNotes)
            .HasMaxLength(2000);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.IncidentDate);
        builder.HasIndex(x => x.Severity);
        builder.HasIndex(x => x.IsResolved);
    }
}

