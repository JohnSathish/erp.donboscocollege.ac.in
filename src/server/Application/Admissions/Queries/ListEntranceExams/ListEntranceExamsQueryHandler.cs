using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.ListEntranceExams;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListEntranceExams;

public sealed class ListEntranceExamsQueryHandler : IRequestHandler<ListEntranceExamsQuery, EntranceExamsListResponse>
{
    private readonly IEntranceExamRepository _examRepository;

    public ListEntranceExamsQueryHandler(IEntranceExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<EntranceExamsListResponse> Handle(ListEntranceExamsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (exams, totalCount) = await _examRepository.GetPagedAsync(
            page,
            pageSize,
            request.IsActive,
            request.ExamDateFrom,
            request.ExamDateTo,
            request.SearchTerm,
            cancellationToken);

        var examDtos = exams.Select(exam => new EntranceExamDto(
            exam.Id,
            exam.ExamName,
            exam.ExamCode,
            exam.Description,
            exam.ExamDate,
            exam.ExamStartTime,
            exam.ExamEndTime,
            exam.Venue,
            exam.VenueAddress,
            exam.Instructions,
            exam.MaxCapacity,
            exam.CurrentRegistrations,
            exam.IsActive,
            exam.CreatedOnUtc,
            exam.CreatedBy,
            exam.UpdatedOnUtc,
            exam.UpdatedBy)).ToList();

        return new EntranceExamsListResponse(examDtos, totalCount, page, pageSize);
    }
}













