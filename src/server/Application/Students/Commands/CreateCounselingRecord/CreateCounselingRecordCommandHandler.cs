using ERP.Application.Students.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.CreateCounselingRecord;

public sealed class CreateCounselingRecordCommandHandler : IRequestHandler<CreateCounselingRecordCommand, Guid>
{
    private readonly ICounselingRecordRepository _counselingRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<CreateCounselingRecordCommandHandler> _logger;

    public CreateCounselingRecordCommandHandler(
        ICounselingRecordRepository counselingRepository,
        IStudentRepository studentRepository,
        ILogger<CreateCounselingRecordCommandHandler> logger)
    {
        _counselingRepository = counselingRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateCounselingRecordCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for counseling record creation.", request.StudentId);
            throw new InvalidOperationException($"Student with ID {request.StudentId} not found.");
        }

        var record = new Domain.Students.Entities.CounselingRecord(
            request.StudentId,
            request.SessionType,
            request.SessionDate,
            request.CounselorName,
            request.CounselorId,
            request.Location,
            request.CreatedBy);

        var createdRecord = await _counselingRepository.AddAsync(record, cancellationToken);

        _logger.LogInformation("Counseling record {RecordId} created for student {StudentNumber} (ID: {StudentId}), Type: {SessionType}, Date: {SessionDate}.",
            createdRecord.Id, student.StudentNumber, student.Id, request.SessionType, request.SessionDate);

        return createdRecord.Id;
    }
}

