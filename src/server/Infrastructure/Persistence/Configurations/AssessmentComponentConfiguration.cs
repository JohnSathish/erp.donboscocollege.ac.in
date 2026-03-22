using ERP.Domain.Examinations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class AssessmentComponentConfiguration : IEntityTypeConfiguration<AssessmentComponent>
{
    public void Configure(EntityTypeBuilder<AssessmentComponent> builder)
    {
        builder.ToTable("AssessmentComponents", "examinations");

        builder.HasKey(ac => ac.Id);

        builder.Property(ac => ac.Id)
            .IsRequired();

        builder.Property(ac => ac.AssessmentId)
            .IsRequired();

        builder.Property(ac => ac.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ac => ac.Code)
            .HasMaxLength(50);

        builder.Property(ac => ac.MaxMarks)
            .IsRequired();

        builder.Property(ac => ac.PassingMarks)
            .IsRequired();

        builder.Property(ac => ac.Weightage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(ac => ac.DisplayOrder)
            .IsRequired();

        builder.Property(ac => ac.Instructions)
            .HasMaxLength(1000);

        builder.Property(ac => ac.CreatedOnUtc)
            .IsRequired();

        builder.Property(ac => ac.CreatedBy)
            .HasMaxLength(200);

        builder.Property(ac => ac.UpdatedOnUtc);

        builder.Property(ac => ac.UpdatedBy)
            .HasMaxLength(200);

        builder.HasOne(ac => ac.Assessment)
            .WithMany(a => a.Components)
            .HasForeignKey(ac => ac.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ac => ac.AssessmentId);
        builder.HasIndex(ac => new { ac.AssessmentId, ac.DisplayOrder });
    }
}





