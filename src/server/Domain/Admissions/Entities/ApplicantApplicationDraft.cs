using ERP.Domain.Admissions.Entities;

namespace ERP.Domain.Admissions.Entities;

public class ApplicantApplicationDraft
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid AccountId { get; private set; }

    public string Data { get; private set; } = string.Empty;

    public DateTime UpdatedOnUtc { get; private set; } = DateTime.UtcNow;

    public StudentApplicantAccount Account { get; private set; } = null!;

    private ApplicantApplicationDraft()
    {
    }

    public ApplicantApplicationDraft(Guid accountId, string data)
    {
        AccountId = accountId;
        Data = data;
    }

    public void Update(string data, DateTime updatedOnUtc)
    {
        Data = data;
        UpdatedOnUtc = updatedOnUtc;
    }
}





