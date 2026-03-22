using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetProgramById;

public sealed record GetProgramByIdQuery(Guid Id) : IRequest<ProgramDto?>;









