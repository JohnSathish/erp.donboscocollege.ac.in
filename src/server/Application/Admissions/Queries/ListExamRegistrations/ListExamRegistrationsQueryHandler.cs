using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.ListExamRegistrations;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListExamRegistrations;

public sealed class ListExamRegistrationsQueryHandler : IRequestHandler<ListExamRegistrationsQuery, ExamRegistrationsListResponse>
{
    private readonly IExamRegistrationRepository _registrationRepository;

    public ListExamRegistrationsQueryHandler(IExamRegistrationRepository registrationRepository)
    {
        _registrationRepository = registrationRepository;
    }

    public async Task<ExamRegistrationsListResponse> Handle(ListExamRegistrationsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (registrations, totalCount) = await _registrationRepository.GetPagedByExamAsync(
            request.ExamId,
            page,
            pageSize,
            request.IsPresent,
            request.SearchTerm,
            cancellationToken);

        var registrationDtos = registrations.Select(reg => new ExamRegistrationDto(
            reg.Id,
            reg.ExamId,
            reg.Exam?.ExamName ?? "Unknown Exam",
            reg.Exam?.ExamCode ?? "N/A",
            reg.ApplicantAccountId,
            reg.ApplicantAccount?.FullName ?? "Unknown Applicant",
            reg.ApplicantAccount?.UniqueId ?? "N/A",
            reg.HallTicketNumber,
            reg.IsPresent,
            reg.Score,
            reg.RegisteredOnUtc,
            reg.RegisteredBy,
            reg.AttendanceMarkedOnUtc,
            reg.AttendanceMarkedBy,
            reg.ScoreEnteredOnUtc,
            reg.ScoreEnteredBy)).ToList();

        return new ExamRegistrationsListResponse(registrationDtos, totalCount, page, pageSize);
    }
}





