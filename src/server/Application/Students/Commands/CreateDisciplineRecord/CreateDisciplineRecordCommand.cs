using MediatR;
using ERP.Domain.Students.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Students.Commands.CreateDisciplineRecord;

public sealed record CreateDisciplineRecordCommand(
    Guid StudentId,
    [Required] string IncidentType,
    [Required] string Description,
    [Required] DateTime IncidentDate,
    [Required] DisciplineSeverity Severity,
    string? Location = null,
    string? ReportedBy = null,
    string? Witnesses = null,
    string? CreatedBy = null) : IRequest<Guid>;

