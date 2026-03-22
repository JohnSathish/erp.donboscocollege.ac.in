using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class ClassXiiSubjectMasterConfiguration : IEntityTypeConfiguration<ClassXiiSubjectMaster>
{
    public void Configure(EntityTypeBuilder<ClassXiiSubjectMaster> builder)
    {
        builder.ToTable("subjects_master", "admissions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BoardCode).HasMaxLength(16).IsRequired();
        builder.Property(x => x.StreamCode).HasMaxLength(16).IsRequired();
        builder.Property(x => x.SubjectName).HasMaxLength(256).IsRequired();
        builder.Property(x => x.SortOrder).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.HasIndex(x => new { x.BoardCode, x.StreamCode, x.SubjectName }).IsUnique();
        builder.HasIndex(x => new { x.BoardCode, x.StreamCode, x.IsActive });
    }
}
