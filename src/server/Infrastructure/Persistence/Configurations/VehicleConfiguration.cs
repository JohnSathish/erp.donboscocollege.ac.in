using ERP.Domain.Transport.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles", "transport");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.VehicleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.VehicleNumber)
            .IsUnique();

        builder.Property(x => x.VehicleType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Make)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Color)
            .HasMaxLength(50);

        builder.Property(x => x.RegistrationNumber)
            .HasMaxLength(50);

        builder.Property(x => x.InsuranceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.DriverName)
            .HasMaxLength(200);

        builder.Property(x => x.DriverContactNumber)
            .HasMaxLength(32);

        builder.Property(x => x.Route)
            .HasMaxLength(200);

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




