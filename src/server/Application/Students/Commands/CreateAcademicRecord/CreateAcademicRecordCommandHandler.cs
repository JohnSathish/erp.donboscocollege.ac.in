using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.CreateAcademicRecord;

public sealed class CreateAcademicRecordCommandHandler : IRequestHandler<CreateAcademicRecordCommand, CreateAcademicRecordResult>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly ILogger<CreateAcademicRecordCommandHandler> _logger;

    public CreateAcademicRecordCommandHandler(
        IStudentRepository studentRepository,
        IAcademicHistoryRepository academicHistoryRepository,
        ILogger<CreateAcademicRecordCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _academicHistoryRepository = academicHistoryRepository;
        _logger = logger;
    }

    public async Task<CreateAcademicRecordResult> Handle(CreateAcademicRecordCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            return new CreateAcademicRecordResult(
                Guid.Empty,
                request.StudentId,
                request.AcademicYear,
                request.Semester,
                false,
                "Student not found.");
        }

        try
        {
            var record = new AcademicRecord(
                request.StudentId,
                request.AcademicYear,
                request.Semester,
                request.TermId,
                request.CreatedBy);

            await _academicHistoryRepository.AddAcademicRecordAsync(record, cancellationToken);

            _logger.LogInformation(
                "Created academic record for StudentId: {StudentId}, AcademicYear: {AcademicYear}, Semester: {Semester}",
                request.StudentId,
                request.AcademicYear,
                request.Semester);

            return new CreateAcademicRecordResult(
                record.Id,
                record.StudentId,
                record.AcademicYear,
                record.Semester,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to create academic record for StudentId: {StudentId}",
                request.StudentId);

            return new CreateAcademicRecordResult(
                Guid.Empty,
                request.StudentId,
                request.AcademicYear,
                request.Semester,
                false,
                ex.Message);
        }
    }
}

