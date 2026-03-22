using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.CreateCourse;

public sealed class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Guid>
{
    private readonly ICourseRepository _repository;

    public CreateCourseCommandHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var existingCourse = await _repository.GetByCodeAsync(request.Code, cancellationToken);
        if (existingCourse != null)
        {
            throw new InvalidOperationException($"Course with code '{request.Code}' already exists.");
        }

        var course = new Course(
            request.Code,
            request.Name,
            request.CreditHours,
            request.ProgramId,
            request.Description,
            request.Prerequisites,
            request.CreatedBy);

        await _repository.AddAsync(course, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return course.Id;
    }
}









