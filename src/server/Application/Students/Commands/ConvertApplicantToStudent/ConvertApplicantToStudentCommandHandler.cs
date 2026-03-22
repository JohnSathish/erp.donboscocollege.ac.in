using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.ConvertApplicantToStudent;

public sealed class ConvertApplicantToStudentCommandHandler : IRequestHandler<ConvertApplicantToStudentCommand, Guid>
{
    private readonly IApplicantAccountRepository _applicantAccountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<ConvertApplicantToStudentCommandHandler> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public ConvertApplicantToStudentCommandHandler(
        IApplicantAccountRepository applicantAccountRepository,
        IApplicantApplicationRepository applicationRepository,
        IStudentRepository studentRepository,
        ILogger<ConvertApplicantToStudentCommandHandler> logger)
    {
        _applicantAccountRepository = applicantAccountRepository;
        _applicationRepository = applicationRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(ConvertApplicantToStudentCommand request, CancellationToken cancellationToken)
    {
        // Check if applicant account exists and is enrolled
        var applicantAccount = await _applicantAccountRepository.GetByIdAsync(request.ApplicantAccountId, cancellationToken);
        if (applicantAccount == null)
        {
            throw new InvalidOperationException($"Applicant account with ID '{request.ApplicantAccountId}' not found.");
        }

        if (applicantAccount.Status != ApplicationStatus.Enrolled)
        {
            throw new InvalidOperationException(
                $"Cannot convert applicant to student. Application status is '{applicantAccount.Status}'. Only enrolled applicants can be converted.");
        }

        // Check if student already exists for this applicant
        var existingStudent = await _studentRepository.GetByApplicantAccountIdAsync(request.ApplicantAccountId, cancellationToken);
        if (existingStudent != null)
        {
            throw new InvalidOperationException(
                $"Student already exists for applicant account '{request.ApplicantAccountId}'. Student Number: {existingStudent.StudentNumber}");
        }

        // Check if student number is already taken
        var existingStudentNumber = await _studentRepository.GetByStudentNumberAsync(request.StudentNumber, cancellationToken);
        if (existingStudentNumber != null)
        {
            throw new InvalidOperationException($"Student number '{request.StudentNumber}' is already in use.");
        }

        // Get application details for major/minor subjects
        string? majorSubject = null;
        string? minorSubject = null;
        if (applicantAccount.IsApplicationSubmitted)
        {
            var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(request.ApplicantAccountId, cancellationToken);
            if (draftEntity is not null)
            {
                var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions);
                majorSubject = draft?.Courses?.MajorSubject;
                minorSubject = draft?.Courses?.MinorSubject;
            }
        }

        // Create student from applicant account
        var student = new Student(
            request.ApplicantAccountId,
            request.StudentNumber,
            applicantAccount.FullName,
            applicantAccount.DateOfBirth,
            applicantAccount.Gender,
            applicantAccount.Email,
            applicantAccount.MobileNumber,
            applicantAccount.Shift,
            request.AcademicYear,
            request.ProgramId,
            request.ProgramCode,
            majorSubject,
            minorSubject,
            applicantAccount.UniqueId, // Use application number as admission number
            applicantAccount.PhotoUrl,
            request.CreatedBy);

        await _studentRepository.AddAsync(student, cancellationToken);
        await _studentRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Converted applicant {ApplicationNumber} (AccountId: {AccountId}) to student {StudentNumber} (StudentId: {StudentId})",
            applicantAccount.UniqueId,
            request.ApplicantAccountId,
            request.StudentNumber,
            student.Id);

        return student.Id;
    }
}


