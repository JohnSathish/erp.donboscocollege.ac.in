using ERP.Application.Common.DTOs;
using MediatR;

namespace ERP.Application.Admissions.Commands.RegisterStudentApplicant;

public sealed record RegisterStudentApplicantCommand(
    string FullName,
    DateOnly DateOfBirth,
    string Gender,
    string Email,
    string MobileNumber,
    UploadedFileDto ProfilePhoto) : IRequest<RegisterStudentApplicantResult>;

