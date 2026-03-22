using MediatR;

namespace ERP.Application.Admissions.Commands.ReceiveOfflineAdmissionForm;

public sealed record ReceiveOfflineAdmissionFormCommand(
    string FormNumber,
    string MajorSubject) : IRequest<ReceiveOfflineAdmissionFormResult>;

public sealed record ReceiveOfflineAdmissionFormResult(
    Guid AccountId,
    string FormNumber,
    string StudentName,
    string MajorSubject,
    DateTime ReceivedOnUtc);
