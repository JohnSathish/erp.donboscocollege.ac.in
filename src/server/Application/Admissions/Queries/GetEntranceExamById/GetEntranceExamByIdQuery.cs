using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetEntranceExamById;

public sealed record GetEntranceExamByIdQuery(Guid ExamId) : IRequest<EntranceExamDto?>;













