using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class FeeComponentConfiguration : IEntityTypeConfiguration<FeeComponent>
{
    public void Configure(EntityTypeBuilder<FeeComponent> builder)
    {
        builder.ToTable("FeeComponents", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.FeeStructureId);

        builder.HasOne(x => x.FeeStructure)
            .WithMany(x => x.Components)
            .HasForeignKey(x => x.FeeStructureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}









