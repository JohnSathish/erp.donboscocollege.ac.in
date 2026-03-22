using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Common.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.CreateStudentAndGuardiansFromOffer;

public sealed class CreateStudentAndGuardiansFromOfferCommandHandler : IRequestHandler<CreateStudentAndGuardiansFromOfferCommand, CreateStudentAndGuardiansFromOfferResult>
{
    private readonly IApplicantAccountRepository _applicantAccountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IGuardianRepository _guardianRepository;
    private readonly IUserAccountService _userAccountService;
    private readonly ILogger<CreateStudentAndGuardiansFromOfferCommandHandler> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public CreateStudentAndGuardiansFromOfferCommandHandler(
        IApplicantAccountRepository applicantAccountRepository,
        IApplicantApplicationRepository applicationRepository,
        IStudentRepository studentRepository,
        IGuardianRepository guardianRepository,
        IUserAccountService userAccountService,
        ILogger<CreateStudentAndGuardiansFromOfferCommandHandler> logger)
    {
        _applicantAccountRepository = applicantAccountRepository;
        _applicationRepository = applicationRepository;
        _studentRepository = studentRepository;
        _guardianRepository = guardianRepository;
        _userAccountService = userAccountService;
        _logger = logger;
    }

    public async Task<CreateStudentAndGuardiansFromOfferResult> Handle(CreateStudentAndGuardiansFromOfferCommand request, CancellationToken cancellationToken)
    {
        // Get applicant account
        var applicantAccount = await _applicantAccountRepository.GetByIdAsync(request.ApplicantAccountId, cancellationToken);
        if (applicantAccount == null)
        {
            throw new InvalidOperationException($"Applicant account with ID '{request.ApplicantAccountId}' not found.");
        }

        // Check if student already exists
        var existingStudent = await _studentRepository.GetByApplicantAccountIdAsync(request.ApplicantAccountId, cancellationToken);
        if (existingStudent != null)
        {
            throw new InvalidOperationException(
                $"Student already exists for applicant account '{request.ApplicantAccountId}'. Student Number: {existingStudent.StudentNumber}");
        }

        // Generate or validate student number
        var studentNumber = request.StudentNumber;
        if (string.IsNullOrWhiteSpace(studentNumber))
        {
            studentNumber = await GenerateStudentNumberAsync(request.AcademicYear, cancellationToken);
        }
        else
        {
            // Validate student number is unique
            var existingStudentNumber = await _studentRepository.GetByStudentNumberAsync(studentNumber, cancellationToken);
            if (existingStudentNumber != null)
            {
                throw new InvalidOperationException($"Student number '{studentNumber}' is already in use.");
            }
        }

        // Get application details
        string? majorSubject = null;
        string? minorSubject = null;
        ApplicantApplicationDraftDto? draft = null;

        if (applicantAccount.IsApplicationSubmitted)
        {
            var draftEntity = await _applicationRepository.GetDraftByAccountIdAsync(request.ApplicantAccountId, cancellationToken);
            if (draftEntity is not null)
            {
                draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _serializerOptions);
                majorSubject = draft?.Courses?.MajorSubject;
                minorSubject = draft?.Courses?.MinorSubject;
            }
        }

        // Create student
        var student = new Student(
            applicantAccount.Id,
            studentNumber,
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
            "Created student {StudentNumber} (StudentId: {StudentId}) from applicant {ApplicationNumber}",
            studentNumber,
            student.Id,
            applicantAccount.UniqueId);

        // Create user account for student
        try
        {
            var studentAccountResult = await _userAccountService.CreateStudentAccountAsync(
                student.Id,
                studentNumber,
                student.FullName,
                student.Email,
                student.MobileNumber,
                cancellationToken);

            if (studentAccountResult.Success)
            {
                student.LinkUserAccount(studentAccountResult.UserId, studentAccountResult.Username);
                await _studentRepository.UpdateAsync(student, cancellationToken);

                // Send credentials to student
                await _userAccountService.SendCredentialsAsync(
                    student.Email,
                    student.MobileNumber,
                    studentAccountResult.Username,
                    studentAccountResult.TemporaryPassword,
                    "Student",
                    cancellationToken);

                _logger.LogInformation(
                    "Created and linked user account {UserId} for student {StudentNumber}",
                    studentAccountResult.UserId,
                    studentNumber);
            }
            else
            {
                _logger.LogWarning(
                    "Failed to create user account for student {StudentNumber}: {ErrorMessage}",
                    studentNumber,
                    studentAccountResult.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception while creating user account for student {StudentNumber}",
                studentNumber);
            // Don't fail student creation if user account creation fails
        }

        // Create guardians from application data
        var guardians = new List<Guardian>();
        if (draft?.Contacts != null)
        {
            // Create Father guardian
            if (!string.IsNullOrWhiteSpace(draft.Contacts.Father?.Name) &&
                !string.IsNullOrWhiteSpace(draft.Contacts.Father?.ContactNumber1))
            {
                var father = new Guardian(
                    student.Id,
                    draft.Contacts.Father.Name,
                    "Father",
                    draft.Contacts.Father.ContactNumber1,
                    draft.Contacts.Father.Age,
                    draft.Contacts.Father.Occupation,
                    null,
                    isPrimary: true,
                    request.CreatedBy);
                guardians.Add(father);
            }

            // Create Mother guardian
            if (!string.IsNullOrWhiteSpace(draft.Contacts.Mother?.Name) &&
                !string.IsNullOrWhiteSpace(draft.Contacts.Mother?.ContactNumber1))
            {
                var mother = new Guardian(
                    student.Id,
                    draft.Contacts.Mother.Name,
                    "Mother",
                    draft.Contacts.Mother.ContactNumber1,
                    draft.Contacts.Mother.Age,
                    draft.Contacts.Mother.Occupation,
                    null,
                    isPrimary: guardians.Count == 0, // Primary if no father
                    request.CreatedBy);
                guardians.Add(mother);
            }

            // Create Local Guardian
            if (!string.IsNullOrWhiteSpace(draft.Contacts.LocalGuardian?.Name) &&
                !string.IsNullOrWhiteSpace(draft.Contacts.LocalGuardian?.ContactNumber1))
            {
                var localGuardian = new Guardian(
                    student.Id,
                    draft.Contacts.LocalGuardian.Name,
                    "LocalGuardian",
                    draft.Contacts.LocalGuardian.ContactNumber1,
                    draft.Contacts.LocalGuardian.Age,
                    draft.Contacts.LocalGuardian.Occupation,
                    null,
                    isPrimary: false,
                    request.CreatedBy);
                guardians.Add(localGuardian);
            }
        }

        // Save guardians and create user accounts
        if (guardians.Count > 0)
        {
            await _guardianRepository.AddRangeAsync(guardians, cancellationToken);
            _logger.LogInformation(
                "Created {Count} guardians for student {StudentNumber}",
                guardians.Count,
                studentNumber);

            // Create user accounts for guardians
            foreach (var guardian in guardians)
            {
                try
                {
                    var guardianAccountResult = await _userAccountService.CreateParentAccountAsync(
                        guardian.Id,
                        guardian.Name,
                        guardian.Relationship,
                        guardian.ContactNumber,
                        guardian.Email,
                        student.Id,
                        cancellationToken);

                    if (guardianAccountResult.Success)
                    {
                        guardian.LinkUserAccount(guardianAccountResult.UserId, guardianAccountResult.Username);
                        await _guardianRepository.UpdateAsync(guardian, cancellationToken);

                        // Send credentials to guardian
                        var guardianEmail = guardian.Email ?? $"{guardian.ContactNumber}@guardian.local";
                        await _userAccountService.SendCredentialsAsync(
                            guardianEmail,
                            guardian.ContactNumber,
                            guardianAccountResult.Username,
                            guardianAccountResult.TemporaryPassword,
                            "Parent",
                            cancellationToken);

                        _logger.LogInformation(
                            "Created and linked user account {UserId} for guardian {GuardianName} ({Relationship})",
                            guardianAccountResult.UserId,
                            guardian.Name,
                            guardian.Relationship);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Failed to create user account for guardian {GuardianName}: {ErrorMessage}",
                            guardian.Name,
                            guardianAccountResult.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Exception while creating user account for guardian {GuardianName}",
                        guardian.Name);
                    // Don't fail guardian creation if user account creation fails
                }
            }
        }

        return new CreateStudentAndGuardiansFromOfferResult(
            student.Id,
            studentNumber,
            guardians.Count);
    }

    private async Task<string> GenerateStudentNumberAsync(string academicYear, CancellationToken cancellationToken)
    {
        // Generate student number in format: YYYY-XXXXX (e.g., 2024-00001)
        var year = academicYear.Length >= 4 ? academicYear.Substring(0, 4) : DateTime.UtcNow.Year.ToString();
        var prefix = $"{year}-";

        // Find the highest existing student number with this prefix
        var allStudents = await _studentRepository.GetAllAsync(isActive: null, cancellationToken);
        var maxNumber = 0;

        foreach (var student in allStudents)
        {
            if (student.StudentNumber.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                var numberPart = student.StudentNumber.Substring(prefix.Length);
                if (int.TryParse(numberPart, out var num) && num > maxNumber)
                {
                    maxNumber = num;
                }
            }
        }

        var nextNumber = maxNumber + 1;
        return $"{prefix}{nextNumber:D5}";
    }
}

