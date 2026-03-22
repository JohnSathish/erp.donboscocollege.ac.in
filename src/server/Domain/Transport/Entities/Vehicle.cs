namespace ERP.Domain.Transport.Entities;

public class Vehicle
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string VehicleNumber { get; private set; } = string.Empty;

    public string VehicleType { get; private set; } = string.Empty; // Bus, Van, Car, etc.

    public string Make { get; private set; } = string.Empty; // Manufacturer

    public string Model { get; private set; } = string.Empty;

    public int? Year { get; private set; }

    public string? Color { get; private set; }

    public int Capacity { get; private set; } // Number of seats

    public string? RegistrationNumber { get; private set; }

    public DateTime? RegistrationExpiryDate { get; private set; }

    public string? InsuranceNumber { get; private set; }

    public DateTime? InsuranceExpiryDate { get; private set; }

    public string? DriverName { get; private set; }

    public string? DriverContactNumber { get; private set; }

    public string? Route { get; private set; } // Assigned route

    public VehicleStatus Status { get; private set; } = VehicleStatus.Active;

    public string? Notes { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private Vehicle() { } // For EF Core

    public Vehicle(
        string vehicleNumber,
        string vehicleType,
        string make,
        string model,
        int capacity,
        string? color = null,
        int? year = null,
        string? registrationNumber = null,
        DateTime? registrationExpiryDate = null,
        string? insuranceNumber = null,
        DateTime? insuranceExpiryDate = null,
        string? driverName = null,
        string? driverContactNumber = null,
        string? route = null,
        string? notes = null,
        string? createdBy = null)
    {
        VehicleNumber = vehicleNumber;
        VehicleType = vehicleType;
        Make = make;
        Model = model;
        Capacity = capacity;
        Color = color;
        Year = year;
        RegistrationNumber = registrationNumber;
        RegistrationExpiryDate = registrationExpiryDate;
        InsuranceNumber = insuranceNumber;
        InsuranceExpiryDate = insuranceExpiryDate;
        DriverName = driverName;
        DriverContactNumber = driverContactNumber;
        Route = route;
        Notes = notes;
        Status = VehicleStatus.Active;
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string vehicleType,
        string make,
        string model,
        int capacity,
        string? color,
        int? year,
        string? registrationNumber,
        DateTime? registrationExpiryDate,
        string? insuranceNumber,
        DateTime? insuranceExpiryDate,
        string? driverName,
        string? driverContactNumber,
        string? route,
        string? notes,
        string? updatedBy = null)
    {
        VehicleType = vehicleType;
        Make = make;
        Model = model;
        Capacity = capacity;
        Color = color;
        Year = year;
        RegistrationNumber = registrationNumber;
        RegistrationExpiryDate = registrationExpiryDate;
        InsuranceNumber = insuranceNumber;
        InsuranceExpiryDate = insuranceExpiryDate;
        DriverName = driverName;
        DriverContactNumber = driverContactNumber;
        Route = route;
        Notes = notes;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateStatus(VehicleStatus status, string? updatedBy = null)
    {
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum VehicleStatus
{
    Active,
    Inactive,
    UnderMaintenance,
    Retired
}




