using ERP.Application.Admissions.ViewModels;

namespace ERP.Application.Admissions.Interfaces;

public interface IAdmissionsReadRepository
{
    Task<ApplicantDto?> GetApplicantByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ApplicantDto>> ListApplicantsAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ApplicantDto>> ListAllApplicantsAsync(CancellationToken cancellationToken = default);
}


