using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListClassXiiSubjects;

public sealed record ListClassXiiSubjectsQuery(string Board, string Stream)
    : IRequest<IReadOnlyList<ClassXiiSubjectOptionDto>>;
