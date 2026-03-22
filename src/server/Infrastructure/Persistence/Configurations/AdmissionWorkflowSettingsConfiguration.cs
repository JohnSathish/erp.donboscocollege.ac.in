using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class AdmissionWorkflowSettingsConfiguration : IEntityTypeConfiguration<AdmissionWorkflowSettings>
{
    public void Configure(EntityTypeBuilder<AdmissionWorkflowSettings> builder)
    {
        builder.ToTable("AdmissionWorkflowSettings", "admissions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.MeritClassXiiCutoffPercentage).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.UpdatedOnUtc).IsRequired();
        builder.Property(x => x.UpdatedBy).HasMaxLength(256);
    }
}
