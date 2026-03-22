using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.ToggleCourseStatus;

public sealed class ToggleCourseStatusCommandHandler : IRequestHandler<ToggleCourseStatusCommand, bool>
{
    private readonly ICourseRepository _repository;

    public ToggleCourseStatusCommandHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleCourseStatusCommand request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
        {
            return false;
        }

        course.ToggleStatus(request.UpdatedBy);
        await _repository.UpdateAsync(course, cancellationToken);
        return true;
    }
}









