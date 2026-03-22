using ERP.Domain.Hostel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class HostelRoomConfiguration : IEntityTypeConfiguration<HostelRoom>
{
    public void Configure(EntityTypeBuilder<HostelRoom> builder)
    {
        builder.ToTable("HostelRooms", "hostel");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoomNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => new { x.BlockName, x.RoomNumber })
            .IsUnique();

        builder.Property(x => x.BlockName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.FloorNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.RoomType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Facilities)
            .HasMaxLength(500);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

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




