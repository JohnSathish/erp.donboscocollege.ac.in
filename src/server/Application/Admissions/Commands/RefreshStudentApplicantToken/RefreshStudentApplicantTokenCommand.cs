using ERP.Application.Admissions.Commands.LoginStudentApplicant;
using MediatR;

namespace ERP.Application.Admissions.Commands.RefreshStudentApplicantToken;

public sealed record RefreshStudentApplicantTokenCommand(string RefreshToken) : IRequest<LoginStudentApplicantResult>;

