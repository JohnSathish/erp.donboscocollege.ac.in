namespace ERP.Application.Admissions.Interfaces;

public interface IAdmissionWorkflowSettingsService
{
    /// <summary>
    /// Effective minimum Class XII % (database row if present, otherwise appsettings default).
    /// </summary>
    Task<decimal> GetMeritClassXiiCutoffPercentageAsync(CancellationToken cancellationToken = default);

    Task UpdateMeritClassXiiCutoffPercentageAsync(
        decimal value,
        string? updatedBy,
        CancellationToken cancellationToken = default);
}
