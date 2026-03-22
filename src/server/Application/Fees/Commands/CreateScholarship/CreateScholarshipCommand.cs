using MediatR;
using ERP.Domain.Fees.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Fees.Commands.CreateScholarship;

public sealed record CreateScholarshipCommand(
    Guid StudentId,
    [Required] string ScholarshipName,
    [Required] ScholarshipType Type,
    [Required] string AcademicYear,
    [Required] DateTime EffectiveFrom,
    DateTime? EffectiveTo = null,
    decimal? Percentage = null,
    decimal? FixedAmount = null,
    string? Description = null,
    string? SponsorName = null,
    string? ApprovalReference = null,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<Guid>;

