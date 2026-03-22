using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteCourse;

public sealed record DeleteCourseCommand(Guid Id) : IRequest<bool>;









