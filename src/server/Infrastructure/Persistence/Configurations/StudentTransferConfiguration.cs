using ERP.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class StudentTransferConfiguration : IEntityTypeConfiguration<StudentTransfer>
{
    public void Configure(EntityTypeBuilder<StudentTransfer> builder)
    {
        builder.ToTable("StudentTransfers", "students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.FromProgramCode)
            .HasMaxLength(32);

        builder.Property(x => x.ToProgramCode)
            .HasMaxLength(32);

        builder.Property(x => x.FromShift)
            .HasMaxLength(32);

        builder.Property(x => x.ToShift)
            .HasMaxLength(32);

        builder.Property(x => x.FromSection)
            .HasMaxLength(64);

        builder.Property(x => x.ToSection)
            .HasMaxLength(64);

        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(1000);

        builder.Property(x => x.RequestedBy)
            .HasMaxLength(256);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.RequestedOnUtc);
    }
}

