namespace ERP.Domain.Admissions.Entities;

public class AdminUser
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Username { get; private set; }

    public string Email { get; private set; }

    public string FullName { get; private set; }

    public string PasswordHash { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public DateTime? LastLoginOnUtc { get; private set; }

    private AdminUser()
    {
        Username = string.Empty;
        Email = string.Empty;
        FullName = string.Empty;
        PasswordHash = string.Empty;
    }

    public AdminUser(
        string username,
        string email,
        string fullName,
        string passwordHash)
    {
        Username = username;
        Email = email;
        FullName = fullName;
        PasswordHash = passwordHash;
    }

    public void UpdateLastLogin()
    {
        LastLoginOnUtc = DateTime.UtcNow;
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}














