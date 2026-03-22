using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.AssignSelectionListRound;

public sealed record AssignSelectionListRoundCommand(
    Guid ApplicationId,
    AdmissionSelectionListRound Round) : IRequest<AssignSelectionListRoundResult>;

public sealed record AssignSelectionListRoundResult(
    Guid ApplicationId,
    string FormNumber,
    AdmissionSelectionListRound Round);
