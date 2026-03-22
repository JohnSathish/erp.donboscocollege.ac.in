using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class ApplicantApplicationDraftConfiguration : IEntityTypeConfiguration<ApplicantApplicationDraft>
{
    public void Configure(EntityTypeBuilder<ApplicantApplicationDraft> builder)
    {
        builder.ToTable("ApplicantApplicationDrafts", "admissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Data)
            .IsRequired();

        builder.Property(x => x.UpdatedOnUtc)
            .IsRequired();

        builder.HasIndex(x => x.AccountId)
            .IsUnique();

        builder.HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}





