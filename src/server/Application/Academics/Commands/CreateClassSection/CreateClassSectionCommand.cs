using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Academics.Commands.CreateClassSection;

public sealed record CreateClassSectionCommand(
    [Required] string SectionName,
    [Required] Guid CourseId,
    [Required] string AcademicYear,
    [Required] string Shift,
    [Required] int Capacity,
    Guid? TermId = null,
    Guid? TeacherId = null,
    string? TeacherName = null,
    string? RoomNumber = null,
    string? Building = null,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<Guid>;

