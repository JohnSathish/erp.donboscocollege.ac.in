using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class MeritScoreConfiguration : IEntityTypeConfiguration<MeritScore>
{
    public void Configure(EntityTypeBuilder<MeritScore> builder)
    {
        builder.ToTable("MeritScores", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId)
            .IsRequired();

        builder.HasIndex(x => x.AccountId)
            .IsUnique();

        builder.Property(x => x.ApplicationNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.HasIndex(x => x.ApplicationNumber);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.ClassXIIPercentage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(x => x.CuetScore)
            .HasPrecision(5, 2);

        builder.Property(x => x.EntranceExamScore)
            .HasPrecision(5, 2);

        builder.Property(x => x.TotalScore)
            .IsRequired()
            .HasPrecision(8, 2);

        builder.Property(x => x.Rank)
            .IsRequired();

        builder.HasIndex(x => new { x.Rank, x.Shift, x.MajorSubject });

        builder.Property(x => x.Shift)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.MajorSubject)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.CalculatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CalculatedBy)
            .HasMaxLength(256);
    }
}

