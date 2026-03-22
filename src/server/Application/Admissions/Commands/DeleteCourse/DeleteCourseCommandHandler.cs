using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteCourse;

public sealed class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
{
    private readonly ICourseRepository _repository;

    public DeleteCourseCommandHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
        {
            return false;
        }

        await _repository.DeleteAsync(course, cancellationToken);
        return true;
    }
}









