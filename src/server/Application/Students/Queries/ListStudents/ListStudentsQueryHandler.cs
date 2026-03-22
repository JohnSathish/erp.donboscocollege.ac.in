using ERP.Application.Students.Interfaces;
using ERP.Application.Students.Queries.ListStudents;
using ERP.Application.Students.ViewModels;
using MediatR;

namespace ERP.Application.Students.Queries.ListStudents;

public sealed class ListStudentsQueryHandler : IRequestHandler<ListStudentsQuery, StudentsListResponse>
{
    private readonly IStudentRepository _repository;

    public ListStudentsQueryHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<StudentsListResponse> Handle(ListStudentsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (students, totalCount) = await _repository.GetPagedAsync(
            page,
            pageSize,
            request.IsActive,
            request.ProgramId,
            request.AcademicYear,
            request.SearchTerm,
            cancellationToken);

        var studentDtos = students.Select(s => new StudentDto(
            s.Id,
            s.StudentNumber,
            s.FullName,
            s.DateOfBirth,
            s.Gender,
            s.Email,
            s.MobileNumber,
            s.PhotoUrl,
            s.ProgramId,
            s.ProgramCode,
            s.MajorSubject,
            s.MinorSubject,
            s.Shift,
            s.AcademicYear,
            s.AdmissionNumber,
            s.EnrollmentDate,
            s.Status.ToString(),
            s.CreatedOnUtc,
            s.CreatedBy,
            s.UpdatedOnUtc,
            s.UpdatedBy)).ToList();

        return new StudentsListResponse(studentDtos, totalCount, page, pageSize);
    }
}


