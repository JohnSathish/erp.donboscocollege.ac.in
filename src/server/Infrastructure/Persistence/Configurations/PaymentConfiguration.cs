using ERP.Domain.Fees.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", "fees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.InvoiceId)
            .IsRequired();

        builder.Property(x => x.PaymentNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.Amount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.PaymentGateway)
            .HasMaxLength(64);

        builder.Property(x => x.TransactionId)
            .HasMaxLength(256);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(128);

        builder.Property(x => x.ChequeNumber)
            .HasMaxLength(64);

        builder.Property(x => x.BankName)
            .HasMaxLength(256);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasOne(x => x.Invoice)
            .WithMany()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.InvoiceId);
        builder.HasIndex(x => x.PaymentNumber)
            .IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.PaymentDate);
    }
}

