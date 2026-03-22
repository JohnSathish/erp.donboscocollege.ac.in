using ERP.Domain.Fees.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> builder)
    {
        builder.ToTable("InvoiceLines", "fees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.InvoiceId)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Quantity)
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.InvoiceId);
    }
}

