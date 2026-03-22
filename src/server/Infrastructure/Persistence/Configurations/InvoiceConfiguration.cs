using ERP.Domain.Fees.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices", "fees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.AcademicYear)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Term)
            .HasMaxLength(64);

        builder.Property(x => x.SubTotal)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.DiscountAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.ScholarshipAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.TaxAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.PaidAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.BalanceAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasMany(x => x.Lines)
            .WithOne(x => x.Invoice)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.InvoiceNumber)
            .IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.DueDate);
    }
}

