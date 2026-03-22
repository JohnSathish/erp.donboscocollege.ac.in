using ERP.Application.Admissions.ViewModels;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetPublishedSelectionList;

public sealed record GetPublishedSelectionListQuery(AdmissionSelectionListRound? Round = null)
    : IRequest<IReadOnlyList<PublishedSelectionListEntryDto>>;
