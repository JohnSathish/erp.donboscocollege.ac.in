using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.RegisterApplicantForExam;

public sealed class RegisterApplicantForExamCommandHandler : IRequestHandler<RegisterApplicantForExamCommand, Guid>
{
    private readonly IEntranceExamRepository _examRepository;
    private readonly IExamRegistrationRepository _registrationRepository;
    private readonly IApplicantAccountRepository _accountRepository;

    public RegisterApplicantForExamCommandHandler(
        IEntranceExamRepository examRepository,
        IExamRegistrationRepository registrationRepository,
        IApplicantAccountRepository accountRepository)
    {
        _examRepository = examRepository;
        _registrationRepository = registrationRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Guid> Handle(RegisterApplicantForExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam == null)
        {
            throw new InvalidOperationException($"Exam with ID '{request.ExamId}' not found.");
        }

        if (!exam.CanRegister())
        {
            throw new InvalidOperationException($"Cannot register for exam '{exam.ExamCode}'. Exam is inactive, full, or has passed.");
        }

        var account = await _accountRepository.GetByIdAsync(request.ApplicantAccountId, cancellationToken);
        if (account == null)
        {
            throw new InvalidOperationException($"Applicant account with ID '{request.ApplicantAccountId}' not found.");
        }

        // Check if already registered
        var existingRegistration = await _registrationRepository.GetByExamAndApplicantAsync(
            request.ExamId,
            request.ApplicantAccountId,
            cancellationToken);
        if (existingRegistration != null)
        {
            throw new InvalidOperationException($"Applicant is already registered for exam '{exam.ExamCode}'.");
        }

        var registration = new ExamRegistration(
            request.ExamId,
            request.ApplicantAccountId,
            request.RegisteredBy);

        // Generate hall ticket number after registration is created
        registration.GenerateHallTicketNumber(exam.ExamCode, account.UniqueId);

        exam.AddRegistration(registration);
        await _registrationRepository.AddAsync(registration, cancellationToken);
        await _examRepository.SaveChangesAsync(cancellationToken);

        return registration.Id;
    }
}

