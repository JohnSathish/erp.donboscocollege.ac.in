namespace ERP.Domain.Admissions.Entities;

/// <summary>Singleton row (Id = 1) storing admin-editable admissions thresholds.</summary>
public sealed class AdmissionWorkflowSettings
{
    public const int SingletonRowId = 1;

    public int Id { get; private set; } = SingletonRowId;

    /// <summary>Minimum Class XII % for merit list and direct admission (same business rule).</summary>
    public decimal MeritClassXiiCutoffPercentage { get; private set; }

    public DateTime UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private AdmissionWorkflowSettings()
    {
    }

    public static AdmissionWorkflowSettings CreateInitial(decimal meritClassXiiCutoffPercentage, string? updatedBy)
    {
        var row = new AdmissionWorkflowSettings();
        row.Id = SingletonRowId;
        row.MeritClassXiiCutoffPercentage = ClampPct(meritClassXiiCutoffPercentage);
        row.UpdatedOnUtc = DateTime.UtcNow;
        row.UpdatedBy = updatedBy;
        return row;
    }

    public void UpdateMeritCutoff(decimal value, string? updatedBy)
    {
        MeritClassXiiCutoffPercentage = ClampPct(value);
        UpdatedOnUtc = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    private static decimal ClampPct(decimal value) => Math.Clamp(Math.Round(value, 2, MidpointRounding.AwayFromZero), 0m, 100m);
}
