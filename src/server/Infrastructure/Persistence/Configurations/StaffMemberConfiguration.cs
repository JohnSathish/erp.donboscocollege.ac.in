using ERP.Domain.Staff.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public class StaffMemberConfiguration : IEntityTypeConfiguration<StaffMember>
{
    public void Configure(EntityTypeBuilder<StaffMember> builder)
    {
        builder.ToTable("StaffMembers", "staff");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.EmployeeNumber)
            .IsUnique();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(x => x.Email);

        builder.Property(x => x.MobileNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Gender)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Department)
            .HasMaxLength(100);

        builder.Property(x => x.Designation)
            .HasMaxLength(100);

        builder.Property(x => x.EmployeeType)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Address)
            .HasMaxLength(500);

        builder.Property(x => x.EmergencyContactName)
            .HasMaxLength(128);

        builder.Property(x => x.EmergencyContactNumber)
            .HasMaxLength(32);

        builder.Property(x => x.Qualifications)
            .HasMaxLength(500);

        builder.Property(x => x.Specialization)
            .HasMaxLength(200);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(256);
    }
}




