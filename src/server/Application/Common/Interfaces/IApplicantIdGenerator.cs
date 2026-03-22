namespace ERP.Application.Common.Interfaces;

public interface IApplicantIdGenerator
{
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
}

