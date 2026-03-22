using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetOnlineApplicationById;

public sealed record GetOnlineApplicationByIdQuery(Guid AccountId) : IRequest<OnlineApplicationDetailDto?>;


















