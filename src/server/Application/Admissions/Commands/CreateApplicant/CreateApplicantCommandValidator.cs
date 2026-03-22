using FluentValidation;

namespace ERP.Application.Admissions.Commands.CreateApplicant;

public sealed class CreateApplicantCommandValidator : AbstractValidator<CreateApplicantCommand>
{
    public CreateApplicantCommandValidator()
    {
        RuleFor(x => x.ApplicationNumber)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Email)
            .NotEmpty()
            // .EmailAddress() // Email validation temporarily disabled for testing
            .MaximumLength(256);

        RuleFor(x => x.ProgramCode)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .MaximumLength(32);
    }
}


