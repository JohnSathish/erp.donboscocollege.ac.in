using ERP.Domain.Fees.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("Refunds", "fees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.PaymentId)
            .IsRequired();

        builder.Property(x => x.RefundNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.Amount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ReasonDetails)
            .HasMaxLength(1000);

        builder.Property(x => x.ProcessedBy)
            .HasMaxLength(256);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasOne(x => x.Payment)
            .WithMany()
            .HasForeignKey(x => x.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.PaymentId);
        builder.HasIndex(x => x.RefundNumber)
            .IsUnique();
        builder.HasIndex(x => x.Status);
    }
}

