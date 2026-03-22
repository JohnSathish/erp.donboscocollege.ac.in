using ERP.Domain.Academics.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms", "academics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoomNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Building)
            .HasMaxLength(100);

        builder.Property(x => x.Floor)
            .HasMaxLength(50);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(RoomType.Classroom);

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.Property(x => x.HasProjector)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.HasComputerLab)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.HasWhiteboard)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.Equipment)
            .HasMaxLength(1000);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedOnUtc);
        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(x => new { x.RoomNumber, x.Building }).IsUnique();
        builder.HasIndex(x => x.Type);
    }
}

