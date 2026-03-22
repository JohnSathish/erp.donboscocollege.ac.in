using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Academics.Commands.CreateAcademicTerm;

public sealed record CreateAcademicTermCommand(
    [Required] string TermName,
    [Required] string AcademicYear,
    [Required] DateTime StartDate,
    [Required] DateTime EndDate,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<Guid>;

