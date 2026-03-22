namespace ERP.Domain.Students.Entities;

public class Guardian
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid StudentId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Relationship { get; private set; } = string.Empty; // Father, Mother, LocalGuardian, etc.

    public string? Age { get; private set; }

    public string? Occupation { get; private set; }

    public string ContactNumber { get; private set; } = string.Empty;

    public string? Email { get; private set; }

    public bool IsPrimary { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    public Guid? UserAccountId { get; private set; }

    public string? UserAccountUsername { get; private set; }

    private Guardian() { }

    public Guardian(
        Guid studentId,
        string name,
        string relationship,
        string contactNumber,
        string? age = null,
        string? occupation = null,
        string? email = null,
        bool isPrimary = false,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Guardian name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(relationship))
            throw new ArgumentException("Relationship is required.", nameof(relationship));
        if (string.IsNullOrWhiteSpace(contactNumber))
            throw new ArgumentException("Contact number is required.", nameof(contactNumber));

        StudentId = studentId;
        Name = name.Trim();
        Relationship = relationship.Trim();
        ContactNumber = contactNumber.Trim();
        Age = string.IsNullOrWhiteSpace(age) ? null : age.Trim();
        Occupation = string.IsNullOrWhiteSpace(occupation) ? null : occupation.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
        IsPrimary = isPrimary;
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string name,
        string contactNumber,
        string? age = null,
        string? occupation = null,
        string? email = null,
        string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Guardian name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(contactNumber))
            throw new ArgumentException("Contact number is required.", nameof(contactNumber));

        Name = name.Trim();
        ContactNumber = contactNumber.Trim();
        Age = string.IsNullOrWhiteSpace(age) ? null : age.Trim();
        Occupation = string.IsNullOrWhiteSpace(occupation) ? null : occupation.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void SetAsNonPrimary()
    {
        IsPrimary = false;
    }

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Activate(string? updatedBy = null)
    {
        IsActive = true;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void LinkUserAccount(Guid userAccountId, string username)
    {
        if (userAccountId == Guid.Empty)
            throw new ArgumentException("User account ID cannot be empty.", nameof(userAccountId));
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.", nameof(username));

        UserAccountId = userAccountId;
        UserAccountUsername = username.Trim();
    }
}

