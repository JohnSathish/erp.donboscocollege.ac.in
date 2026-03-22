using ERP.Domain.Examinations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class MarkEntryConfiguration : IEntityTypeConfiguration<MarkEntry>
{
    public void Configure(EntityTypeBuilder<MarkEntry> builder)
    {
        builder.ToTable("MarkEntries", "examinations");

        builder.HasKey(me => me.Id);

        builder.Property(me => me.Id)
            .IsRequired();

        builder.Property(me => me.AssessmentComponentId)
            .IsRequired();

        builder.Property(me => me.StudentId)
            .IsRequired();

        builder.Property(me => me.MarksObtained)
            .IsRequired()
            .HasPrecision(6, 2);

        builder.Property(me => me.Percentage)
            .HasPrecision(5, 2);

        builder.Property(me => me.Grade)
            .HasMaxLength(10);

        builder.Property(me => me.Remarks)
            .HasMaxLength(500);

        builder.Property(me => me.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(me => me.IsAbsent)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(me => me.IsExempted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(me => me.EnteredOnUtc);

        builder.Property(me => me.EnteredBy)
            .HasMaxLength(200);

        builder.Property(me => me.ApprovedOnUtc);

        builder.Property(me => me.ApprovedBy)
            .HasMaxLength(200);

        builder.Property(me => me.ModifiedOnUtc);

        builder.Property(me => me.ModifiedBy)
            .HasMaxLength(200);

        builder.Property(me => me.CreatedOnUtc)
            .IsRequired();

        builder.Property(me => me.CreatedBy)
            .HasMaxLength(200);

        builder.Property(me => me.UpdatedOnUtc);

        builder.Property(me => me.UpdatedBy)
            .HasMaxLength(200);

        builder.HasOne(me => me.AssessmentComponent)
            .WithMany(ac => ac.MarkEntries)
            .HasForeignKey(me => me.AssessmentComponentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(me => new { me.AssessmentComponentId, me.StudentId })
            .IsUnique();

        builder.HasIndex(me => me.StudentId);
        builder.HasIndex(me => me.Status);
    }
}





