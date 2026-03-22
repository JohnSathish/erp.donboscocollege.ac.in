using MediatR;

namespace ERP.Application.Admissions.Queries.GetOfflineFormReceiptPdf;

public sealed record GetOfflineFormReceiptPdfQuery(string FormNumber) : IRequest<OfflineFormReceiptPdfQueryResult>;

public sealed record OfflineFormReceiptPdfQueryResult(byte[] Content, string FileName);
