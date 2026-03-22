using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.ViewModels;

public sealed record PublishedSelectionListEntryDto(
    string FormNumber,
    string FullName,
    string? MajorSubject,
    AdmissionSelectionListRound Round,
    DateTime PublishedOnUtc);
