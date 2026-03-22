using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.CreateDisciplineRecord;

public sealed class CreateDisciplineRecordCommandHandler : IRequestHandler<CreateDisciplineRecordCommand, Guid>
{
    private readonly IDisciplineRecordRepository _disciplineRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<CreateDisciplineRecordCommandHandler> _logger;

    public CreateDisciplineRecordCommandHandler(
        IDisciplineRecordRepository disciplineRepository,
        IStudentRepository studentRepository,
        ILogger<CreateDisciplineRecordCommandHandler> logger)
    {
        _disciplineRepository = disciplineRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateDisciplineRecordCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for discipline record creation.", request.StudentId);
            throw new InvalidOperationException($"Student with ID {request.StudentId} not found.");
        }

        var record = new DisciplineRecord(
            request.StudentId,
            request.IncidentType,
            request.Description,
            request.IncidentDate,
            request.Severity,
            request.Location,
            request.ReportedBy,
            request.Witnesses,
            request.CreatedBy);

        var createdRecord = await _disciplineRepository.AddAsync(record, cancellationToken);

        _logger.LogInformation("Discipline record {RecordId} created for student {StudentNumber} (ID: {StudentId}), Severity: {Severity}.",
            createdRecord.Id, student.StudentNumber, student.Id, request.Severity);

        return createdRecord.Id;
    }
}

