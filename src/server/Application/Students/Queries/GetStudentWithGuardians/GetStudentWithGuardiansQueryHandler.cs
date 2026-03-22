using ERP.Application.Students.Interfaces;
using MediatR;

namespace ERP.Application.Students.Queries.GetStudentWithGuardians;

public sealed class GetStudentWithGuardiansQueryHandler : IRequestHandler<GetStudentWithGuardiansQuery, StudentWithGuardiansDto?>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IGuardianRepository _guardianRepository;

    public GetStudentWithGuardiansQueryHandler(
        IStudentRepository studentRepository,
        IGuardianRepository guardianRepository)
    {
        _studentRepository = studentRepository;
        _guardianRepository = guardianRepository;
    }

    public async Task<StudentWithGuardiansDto?> Handle(GetStudentWithGuardiansQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            return null;
        }

        var guardians = await _guardianRepository.GetByStudentIdAsync(request.StudentId, cancellationToken);
        var guardianDtos = guardians.Select(g => new GuardianDto(
            g.Id,
            g.Name,
            g.Relationship,
            g.Age,
            g.Occupation,
            g.ContactNumber,
            g.Email,
            g.IsPrimary,
            g.IsActive)).ToList();

        return new StudentWithGuardiansDto(
            student.Id,
            student.StudentNumber,
            student.FullName,
            student.DateOfBirth,
            student.Gender,
            student.Email,
            student.MobileNumber,
            student.PhotoUrl,
            student.ProgramId,
            student.ProgramCode,
            student.MajorSubject,
            student.MinorSubject,
            student.Shift,
            student.AcademicYear,
            student.AdmissionNumber,
            student.EnrollmentDate,
            student.Status.ToString(),
            student.CreatedOnUtc,
            student.CreatedBy,
            guardianDtos);
    }
}

