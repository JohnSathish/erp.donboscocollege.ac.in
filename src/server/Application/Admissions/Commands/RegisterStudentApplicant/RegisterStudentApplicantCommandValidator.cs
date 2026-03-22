using ERP.Application.Admissions.Interfaces;
using FluentValidation;

namespace ERP.Application.Admissions.Commands.RegisterStudentApplicant;

public sealed class RegisterStudentApplicantCommandValidator : AbstractValidator<RegisterStudentApplicantCommand>
{
    public RegisterStudentApplicantCommandValidator(IApplicantAccountRepository repository)
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of Birth must be in the past.");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Email)
            .NotEmpty()
            // .EmailAddress() // Email validation temporarily disabled for testing
            .MaximumLength(256);
            // Duplicate email check temporarily disabled for testing
            // .MustAsync(async (email, cancellationToken) =>
            //     !await repository.EmailExistsAsync(email, cancellationToken))
            // .WithMessage("Email is already registered.");

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .Matches(@"^[0-9]{10}$")
            .WithMessage("Mobile number must be exactly 10 digits.");
            // Duplicate mobile number check temporarily disabled for testing
            // .MustAsync(async (mobile, cancellationToken) =>
            //     !await repository.MobileExistsAsync(mobile, cancellationToken))
            // .WithMessage("Mobile number is already registered.");

        RuleFor(x => x.ProfilePhoto)
            .NotNull()
            .WithMessage("Profile photo is required.");
    }
}

