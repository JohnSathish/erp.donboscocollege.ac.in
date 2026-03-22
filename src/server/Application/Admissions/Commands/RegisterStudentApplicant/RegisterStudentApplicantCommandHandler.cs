using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Events;
using ERP.Application.Common.DTOs;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.RegisterStudentApplicant;

public sealed class RegisterStudentApplicantCommandHandler : IRequestHandler<RegisterStudentApplicantCommand, RegisterStudentApplicantResult>
{
    private readonly IApplicantAccountRepository _repository;
    private readonly IApplicantIdGenerator _idGenerator;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IApplicantPasswordHasher _passwordHasher;
    private readonly IApplicantPhotoStorageService _photoStorageService;
    private readonly IPublisher _publisher;
    private const string PendingShiftSelection = "Pending Selection";

    public RegisterStudentApplicantCommandHandler(
        IApplicantAccountRepository repository,
        IApplicantIdGenerator idGenerator,
        IPasswordGenerator passwordGenerator,
        IApplicantPasswordHasher passwordHasher,
        IApplicantPhotoStorageService photoStorageService,
        IPublisher publisher)
    {
        _repository = repository;
        _idGenerator = idGenerator;
        _passwordGenerator = passwordGenerator;
        _passwordHasher = passwordHasher;
        _photoStorageService = photoStorageService;
        _publisher = publisher;
    }

    public async Task<RegisterStudentApplicantResult> Handle(RegisterStudentApplicantCommand request, CancellationToken cancellationToken)
    {
        // Duplicate email check temporarily disabled for testing
        // if (await _repository.EmailExistsAsync(request.Email, cancellationToken))
        // {
        //     throw new InvalidOperationException("An applicant account already exists for this email address.");
        // }

        // Duplicate mobile number check temporarily disabled for testing
        // if (await _repository.MobileExistsAsync(request.MobileNumber, cancellationToken))
        // {
        //     throw new InvalidOperationException("An applicant account already exists for this mobile number.");
        // }

        var uniqueId = await _idGenerator.GenerateAsync(cancellationToken);
        var tempPassword = _passwordGenerator.GenerateTemporaryPassword();

        var account = new StudentApplicantAccount(
            uniqueId,
            request.FullName,
            request.DateOfBirth,
            request.Gender,
            request.Email,
            request.MobileNumber,
            PendingShiftSelection);

        // Profile photo is now mandatory, so it should always be present
        var photoUrl = await _photoStorageService.SaveProfilePhotoAsync(account.Id, request.ProfilePhoto, cancellationToken);
        account.SetPhotoUrl(photoUrl);

        var passwordHash = _passwordHasher.HashPassword(account, tempPassword);
        account.SetPasswordHash(passwordHash);

        await _repository.AddAsync(account, cancellationToken);

        var notification = new StudentApplicantRegisteredEvent(
            account.Id,
            account.UniqueId,
            account.FullName,
            account.Email,
            account.MobileNumber,
            tempPassword);

        await _publisher.Publish(notification, cancellationToken);

        return new RegisterStudentApplicantResult(uniqueId, tempPassword);
    }
}

