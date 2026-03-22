using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteEntranceExam;

public sealed class DeleteEntranceExamCommandHandler : IRequestHandler<DeleteEntranceExamCommand, bool>
{
    private readonly IEntranceExamRepository _examRepository;

    public DeleteEntranceExamCommandHandler(IEntranceExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<bool> Handle(DeleteEntranceExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
        {
            return false;
        }

        // Check if exam has registrations
        if (exam.CurrentRegistrations > 0)
        {
            throw new InvalidOperationException(
                $"Cannot delete exam '{exam.ExamName}' because it has {exam.CurrentRegistrations} registrations. " +
                "Please deactivate the exam instead or remove all registrations first.");
        }

        await _examRepository.DeleteAsync(exam, cancellationToken);
        return true;
    }
}











