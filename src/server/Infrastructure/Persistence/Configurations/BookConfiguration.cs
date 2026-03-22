using ERP.Domain.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", "library");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Isbn)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Isbn);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Author)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Publisher)
            .HasMaxLength(200);

        builder.Property(x => x.Category)
            .HasMaxLength(100);

        builder.Property(x => x.Language)
            .HasMaxLength(50);

        builder.Property(x => x.Location)
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);
    }
}




