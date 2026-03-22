using MediatR;

namespace ERP.Application.Admissions.Commands.CreateApplicant;

public sealed record CreateApplicantCommand(
    string ApplicationNumber,
    string FirstName,
    string LastName,
    string Email,
    DateOnly DateOfBirth,
    string ProgramCode,
    string MobileNumber) : IRequest<Guid>;


