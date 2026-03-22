using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.ToggleExamStatus;

public sealed class ToggleExamStatusCommandHandler : IRequestHandler<ToggleExamStatusCommand, bool>
{
    private readonly IEntranceExamRepository _examRepository;

    public ToggleExamStatusCommandHandler(IEntranceExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<bool> Handle(ToggleExamStatusCommand request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
        {
            return false;
        }

        exam.SetActiveStatus(request.IsActive, request.UpdatedBy);
        await _examRepository.UpdateAsync(exam, cancellationToken);

        return true;
    }
}











