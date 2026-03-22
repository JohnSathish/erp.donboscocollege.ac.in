using FluentValidation;

namespace ERP.Application.Admissions.Commands.LoginStudentApplicant;

public sealed class LoginStudentApplicantCommandValidator : AbstractValidator<LoginStudentApplicantCommand>
{
    public LoginStudentApplicantCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(256);
    }
}

