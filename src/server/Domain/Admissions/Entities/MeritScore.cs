namespace ERP.Domain.Admissions.Entities;

public class MeritScore
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid AccountId { get; private set; }

    public string ApplicationNumber { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    public decimal ClassXIIPercentage { get; private set; }

    public decimal? CuetScore { get; private set; }

    public decimal? EntranceExamScore { get; private set; }

    public decimal TotalScore { get; private set; }

    public int Rank { get; private set; }

    public string Shift { get; private set; } = string.Empty;

    public string MajorSubject { get; private set; } = string.Empty;

    public DateTime CalculatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CalculatedBy { get; private set; }

    private MeritScore() { }

    public MeritScore(
        Guid accountId,
        string applicationNumber,
        string fullName,
        decimal classXIIPercentage,
        decimal? cuetScore,
        decimal? entranceExamScore,
        decimal totalScore,
        int rank,
        string shift,
        string majorSubject,
        string? calculatedBy = null)
    {
        AccountId = accountId;
        ApplicationNumber = applicationNumber;
        FullName = fullName;
        ClassXIIPercentage = classXIIPercentage;
        CuetScore = cuetScore;
        EntranceExamScore = entranceExamScore;
        TotalScore = totalScore;
        Rank = rank;
        Shift = shift;
        MajorSubject = majorSubject;
        CalculatedBy = calculatedBy;
        CalculatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateRank(int newRank)
    {
        Rank = newRank;
    }
}

