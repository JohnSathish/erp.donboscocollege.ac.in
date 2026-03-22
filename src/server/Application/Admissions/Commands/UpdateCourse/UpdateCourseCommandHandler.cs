using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateCourse;

public sealed class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, bool>
{
    private readonly ICourseRepository _repository;

    public UpdateCourseCommandHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
        {
            return false;
        }

        course.Update(
            request.Name,
            request.CreditHours,
            request.ProgramId,
            request.Description,
            request.Prerequisites,
            request.UpdatedBy);

        await _repository.UpdateAsync(course, cancellationToken);
        return true;
    }
}









