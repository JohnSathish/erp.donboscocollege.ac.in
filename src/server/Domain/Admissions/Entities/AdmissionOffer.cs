namespace ERP.Domain.Admissions.Entities;

public enum OfferStatus
{
    Pending,
    Accepted,
    Rejected,
    Expired,
    Withdrawn
}

public class AdmissionOffer
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid AccountId { get; private set; }

    public string ApplicationNumber { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    public int MeritRank { get; private set; }

    public string Shift { get; private set; } = string.Empty;

    public string MajorSubject { get; private set; } = string.Empty;

    public OfferStatus Status { get; private set; } = OfferStatus.Pending;

    public DateTime OfferDate { get; private set; } = DateTime.UtcNow;

    public DateTime ExpiryDate { get; private set; }

    public DateTime? AcceptedOnUtc { get; private set; }

    public DateTime? RejectedOnUtc { get; private set; }

    public string? Remarks { get; private set; }

    public string CreatedBy { get; private set; } = string.Empty;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    private AdmissionOffer() { }

    public AdmissionOffer(
        Guid accountId,
        string applicationNumber,
        string fullName,
        int meritRank,
        string shift,
        string majorSubject,
        DateTime expiryDate,
        string createdBy,
        string? remarks = null)
    {
        AccountId = accountId;
        ApplicationNumber = applicationNumber;
        FullName = fullName;
        MeritRank = meritRank;
        Shift = shift;
        MajorSubject = majorSubject;
        ExpiryDate = expiryDate;
        CreatedBy = createdBy;
        Remarks = remarks;
        OfferDate = DateTime.UtcNow;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Accept()
    {
        if (Status != OfferStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot accept offer in {Status} status.");
        }

        if (DateTime.UtcNow > ExpiryDate)
        {
            throw new InvalidOperationException("Cannot accept expired offer.");
        }

        Status = OfferStatus.Accepted;
        AcceptedOnUtc = DateTime.UtcNow;
    }

    public void Reject(string? reason = null)
    {
        if (Status != OfferStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot reject offer in {Status} status.");
        }

        Status = OfferStatus.Rejected;
        RejectedOnUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Remarks = reason.Trim();
        }
    }

    public void Withdraw(string? remarks = null)
    {
        if (Status == OfferStatus.Accepted)
        {
            throw new InvalidOperationException("Cannot withdraw an accepted offer.");
        }

        Status = OfferStatus.Withdrawn;
        Remarks = remarks;
    }

    public void MarkExpired()
    {
        if (Status == OfferStatus.Pending && DateTime.UtcNow > ExpiryDate)
        {
            Status = OfferStatus.Expired;
        }
    }
}

