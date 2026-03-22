using MediatR;

namespace ERP.Application.Admissions.Commands.CreateStudentAndGuardiansFromOffer;

public sealed record CreateStudentAndGuardiansFromOfferCommand(
    Guid ApplicantAccountId,
    string AcademicYear,
    Guid? ProgramId = null,
    string? ProgramCode = null,
    string? StudentNumber = null,
    string? CreatedBy = null) : IRequest<CreateStudentAndGuardiansFromOfferResult>;

public sealed record CreateStudentAndGuardiansFromOfferResult(
    Guid StudentId,
    string StudentNumber,
    int GuardiansCreated);

