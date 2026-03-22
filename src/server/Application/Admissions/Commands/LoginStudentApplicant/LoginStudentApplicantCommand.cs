using MediatR;

namespace ERP.Application.Admissions.Commands.LoginStudentApplicant;

public sealed record LoginStudentApplicantCommand(string Username, string Password) : IRequest<LoginStudentApplicantResult>;

