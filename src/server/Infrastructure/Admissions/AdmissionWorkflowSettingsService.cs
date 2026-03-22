using System.Data.Common;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Options;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ERP.Infrastructure.Admissions;

public sealed class AdmissionWorkflowSettingsService : IAdmissionWorkflowSettingsService
{
    private readonly ApplicationDbContext _db;
    private readonly IOptions<AdmissionsWorkflowOptions> _options;
    private readonly ILogger<AdmissionWorkflowSettingsService> _logger;

    public AdmissionWorkflowSettingsService(
        ApplicationDbContext db,
        IOptions<AdmissionsWorkflowOptions> options,
        ILogger<AdmissionWorkflowSettingsService> logger)
    {
        _db = db;
        _options = options;
        _logger = logger;
    }

    public async Task<decimal> GetMeritClassXiiCutoffPercentageAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var row = await _db.AdmissionWorkflowSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == AdmissionWorkflowSettings.SingletonRowId, cancellationToken);

            return row?.MeritClassXiiCutoffPercentage ?? _options.Value.DirectAdmissionCutoffPercentage;
        }
        catch (DbException ex)
        {
            _logger.LogWarning(
                ex,
                "AdmissionWorkflowSettings query failed (table missing or DB issue); using appsettings cutoff.");
            return _options.Value.DirectAdmissionCutoffPercentage;
        }
    }

    public async Task UpdateMeritClassXiiCutoffPercentageAsync(
        decimal value,
        string? updatedBy,
        CancellationToken cancellationToken = default)
    {
        var existing = await _db.AdmissionWorkflowSettings
            .FirstOrDefaultAsync(x => x.Id == AdmissionWorkflowSettings.SingletonRowId, cancellationToken);

        if (existing is null)
        {
            await _db.AdmissionWorkflowSettings.AddAsync(
                AdmissionWorkflowSettings.CreateInitial(value, updatedBy),
                cancellationToken);
        }
        else
        {
            existing.UpdateMeritCutoff(value, updatedBy);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
