using ERP.Domain.Hostel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class RoomAllocationConfiguration : IEntityTypeConfiguration<RoomAllocation>
{
    public void Configure(EntityTypeBuilder<RoomAllocation> builder)
    {
        builder.ToTable("RoomAllocations", "hostel");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BedNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.AllocatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.VacatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(x => x.RoomId);
        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.AllocationDate);
    }
}




