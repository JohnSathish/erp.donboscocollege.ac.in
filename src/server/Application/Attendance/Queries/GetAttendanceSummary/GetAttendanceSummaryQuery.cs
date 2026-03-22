using MediatR;
using ERP.Application.Attendance.Interfaces;
using ERP.Domain.Attendance.Entities;

namespace ERP.Application.Attendance.Queries.GetAttendanceSummary;

public sealed record GetAttendanceSummaryQuery(
    Guid PersonId,
    PersonType PersonType,
    string? AcademicYear = null,
    string? Term = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IRequest<AttendanceSummaryDto?>;

