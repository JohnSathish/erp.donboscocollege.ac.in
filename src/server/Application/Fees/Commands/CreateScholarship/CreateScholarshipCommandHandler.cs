using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Commands.CreateScholarship;

public sealed class CreateScholarshipCommandHandler : IRequestHandler<CreateScholarshipCommand, Guid>
{
    private readonly IScholarshipRepository _scholarshipRepository;
    private readonly ILogger<CreateScholarshipCommandHandler> _logger;

    public CreateScholarshipCommandHandler(
        IScholarshipRepository scholarshipRepository,
        ILogger<CreateScholarshipCommandHandler> logger)
    {
        _scholarshipRepository = scholarshipRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateScholarshipCommand request, CancellationToken cancellationToken)
    {
        var scholarship = new Domain.Fees.Entities.Scholarship(
            request.StudentId,
            request.ScholarshipName,
            request.Type,
            request.AcademicYear,
            request.EffectiveFrom,
            request.Percentage,
            request.FixedAmount,
            request.EffectiveTo,
            request.Description,
            request.SponsorName,
            request.ApprovalReference,
            request.Remarks,
            request.CreatedBy);

        var createdScholarship = await _scholarshipRepository.AddAsync(scholarship, cancellationToken);

        _logger.LogInformation("Scholarship {ScholarshipName} created for student {StudentId} (ID: {ScholarshipId}).",
            request.ScholarshipName, request.StudentId, createdScholarship.Id);

        return createdScholarship.Id;
    }
}

