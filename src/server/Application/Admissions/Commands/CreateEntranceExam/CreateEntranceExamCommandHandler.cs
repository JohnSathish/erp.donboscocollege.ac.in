using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.CreateEntranceExam;

public sealed class CreateEntranceExamCommandHandler : IRequestHandler<CreateEntranceExamCommand, Guid>
{
    private readonly IEntranceExamRepository _examRepository;

    public CreateEntranceExamCommandHandler(IEntranceExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<Guid> Handle(CreateEntranceExamCommand request, CancellationToken cancellationToken)
    {
        // Check if exam code already exists
        var existingExam = await _examRepository.GetByCodeAsync(request.ExamCode, cancellationToken);
        if (existingExam != null)
        {
            throw new InvalidOperationException($"An exam with code '{request.ExamCode}' already exists.");
        }

        var exam = new EntranceExam(
            request.ExamName,
            request.ExamCode,
            request.ExamDate,
            request.ExamStartTime,
            request.ExamEndTime,
            request.Venue,
            request.MaxCapacity,
            request.Description,
            request.VenueAddress,
            request.Instructions,
            request.CreatedBy);

        await _examRepository.AddAsync(exam, cancellationToken);
        await _examRepository.SaveChangesAsync(cancellationToken);

        return exam.Id;
    }
}

