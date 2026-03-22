using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateEntranceExam;

public sealed class UpdateEntranceExamCommandHandler : IRequestHandler<UpdateEntranceExamCommand>
{
    private readonly IEntranceExamRepository _examRepository;

    public UpdateEntranceExamCommandHandler(IEntranceExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<Unit> Handle(UpdateEntranceExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam == null)
        {
            throw new InvalidOperationException($"Exam with ID '{request.ExamId}' not found.");
        }

        exam.Update(
            request.ExamName,
            request.ExamDate,
            request.ExamStartTime,
            request.ExamEndTime,
            request.Venue,
            request.MaxCapacity,
            request.Description,
            request.VenueAddress,
            request.Instructions,
            request.UpdatedBy);

        _examRepository.Update(exam);
        await _examRepository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

