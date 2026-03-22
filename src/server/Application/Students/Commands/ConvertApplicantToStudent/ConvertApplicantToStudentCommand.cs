using MediatR;

namespace ERP.Application.Students.Commands.ConvertApplicantToStudent;

public sealed record ConvertApplicantToStudentCommand(
    Guid ApplicantAccountId,
    string StudentNumber,
    string AcademicYear,
    Guid? ProgramId = null,
    string? ProgramCode = null,
    string? CreatedBy = null) : IRequest<Guid>;


