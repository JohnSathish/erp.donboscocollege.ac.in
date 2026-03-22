using ERP.Application.Admissions.Commands.LoginStudentApplicant;
using MediatR;

namespace ERP.Application.Admissions.Commands.ChangeStudentApplicantPassword;

public sealed record ChangeStudentApplicantPasswordCommand(Guid AccountId, string CurrentPassword, string NewPassword) : IRequest<LoginStudentApplicantResult>;

