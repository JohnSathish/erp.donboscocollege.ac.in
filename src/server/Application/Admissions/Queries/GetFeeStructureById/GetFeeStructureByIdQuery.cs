using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetFeeStructureById;

public sealed record GetFeeStructureByIdQuery(Guid Id) : IRequest<FeeStructureDto?>;


