using MediatR;

namespace ERP.Application.Admissions.Commands.MarkExamAttendance;

public sealed record MarkExamAttendanceCommand(
    Guid RegistrationId,
    bool IsPresent,
    string? MarkedBy = null) : IRequest<bool>;











