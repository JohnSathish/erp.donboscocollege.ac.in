using ERP.Domain.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class BookIssueConfiguration : IEntityTypeConfiguration<BookIssue>
{
    public void Configure(EntityTypeBuilder<BookIssue> builder)
    {
        builder.ToTable("BookIssues", "library");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IssuedToType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.IssuedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ReturnedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => x.BookId);
        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.StaffId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.DueDate);
    }
}




