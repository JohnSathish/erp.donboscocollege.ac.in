using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.PublishSelectionList;

public sealed record PublishSelectionListCommand(
    AdmissionSelectionListRound Round,
    bool SendSmsNotifications) : IRequest<PublishSelectionListResult>;

public sealed record PublishSelectionListResult(
    int PublishedCount,
    int SmsSent,
    int SmsFailed);
