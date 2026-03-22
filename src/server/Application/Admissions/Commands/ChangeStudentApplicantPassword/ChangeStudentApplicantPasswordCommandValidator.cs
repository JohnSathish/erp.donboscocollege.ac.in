using FluentValidation;

namespace ERP.Application.Admissions.Commands.ChangeStudentApplicantPassword;

public sealed class ChangeStudentApplicantPasswordCommandValidator : AbstractValidator<ChangeStudentApplicantPasswordCommand>
{
    public ChangeStudentApplicantPasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .MinimumLength(5) // Allow 5-digit temporary password
            .MaximumLength(256);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8) // New password must be stronger for security
            .MaximumLength(256);
    }
}

