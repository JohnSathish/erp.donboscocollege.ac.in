using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class FeeStructureConfiguration : IEntityTypeConfiguration<FeeStructure>
{
    public void Configure(EntityTypeBuilder<FeeStructure> builder)
    {
        builder.ToTable("FeeStructures", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.AcademicYear)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.ProgramId);

        builder.HasIndex(x => x.AcademicYear);

        builder.HasOne(x => x.Program)
            .WithMany()
            .HasForeignKey(x => x.ProgramId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Components)
            .WithOne(x => x.FeeStructure)
            .HasForeignKey(x => x.FeeStructureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}









