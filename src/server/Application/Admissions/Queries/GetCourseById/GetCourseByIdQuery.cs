using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetCourseById;

public sealed record GetCourseByIdQuery(Guid Id) : IRequest<CourseDto?>;









