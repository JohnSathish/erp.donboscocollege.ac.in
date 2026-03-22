using ERP.Application.Students.ViewModels;
using MediatR;

namespace ERP.Application.Students.Queries.GetStudentById;

public sealed record GetStudentByIdQuery(Guid Id) : IRequest<StudentDto?>;


