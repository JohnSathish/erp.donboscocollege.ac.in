using MediatR;

namespace ERP.Application.Admissions.Commands.IssueOfflineAdmissionForm;

public sealed record IssueOfflineAdmissionFormCommand(
    string FormNumber,
    string StudentName,
    string MobileNumber,
    decimal ApplicationFeeAmount) : IRequest<IssueOfflineAdmissionFormResult>;

public sealed record IssueOfflineAdmissionFormResult(
    Guid IssuanceId,
    string FormNumber,
    string StudentName,
    decimal ApplicationFeeAmount,
    DateTime IssuedOnUtc,
    byte[] ReceiptPdfContent,
    string ReceiptFileName);
