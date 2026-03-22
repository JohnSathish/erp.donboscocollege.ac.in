using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.GetEntranceExamById;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetEntranceExamById;

public sealed class GetEntranceExamByIdQueryHandler : IRequestHandler<GetEntranceExamByIdQuery, EntranceExamDto?>
{
    private readonly IEntranceExamRepository _examRepository;

    public GetEntranceExamByIdQueryHandler(IEntranceExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<EntranceExamDto?> Handle(GetEntranceExamByIdQuery request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam == null)
        {
            return null;
        }

        return new EntranceExamDto(
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
            exam.UpdatedBy);
    }
}













