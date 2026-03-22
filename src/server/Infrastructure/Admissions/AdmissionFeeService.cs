using ERP.Application.Admissions.Interfaces;

namespace ERP.Infrastructure.Admissions;

public sealed class AdmissionFeeService : IAdmissionFeeService
{
    // Default admission fee: ₹7,000
    private const decimal DefaultAdmissionFee = 7000m;

    // Science stream admission fee (can be configured differently)
    private const decimal ScienceStreamAdmissionFee = 7000m; // Can be changed if different

    // Stream identifiers (case-insensitive matching)
    private static readonly HashSet<string> ScienceStreamIdentifiers = new(StringComparer.OrdinalIgnoreCase)
    {
        "Science",
        "B.Sc",
        "BSc",
        "Bachelor of Science",
        "Physics",
        "Chemistry",
        "Biology",
        "Mathematics",
        "Computer Science"
    };

    public decimal GetAdmissionFee(string? majorSubject)
    {
        if (string.IsNullOrWhiteSpace(majorSubject))
        {
            return DefaultAdmissionFee;
        }

        var trimmedSubject = majorSubject.Trim();

        // Check if it's a Science stream
        if (IsScienceStream(trimmedSubject))
        {
            return ScienceStreamAdmissionFee;
        }

        // Default fee for all other streams
        return DefaultAdmissionFee;
    }

    public bool IsPaymentAmountValid(decimal? paidAmount, string? majorSubject)
    {
        if (!paidAmount.HasValue || paidAmount.Value <= 0)
        {
            return false;
        }

        var requiredFee = GetAdmissionFee(majorSubject);
        return paidAmount.Value >= requiredFee;
    }

    private static bool IsScienceStream(string majorSubject)
    {
        // Check if the major subject contains any Science stream identifier
        return ScienceStreamIdentifiers.Any(identifier =>
            majorSubject.Contains(identifier, StringComparison.OrdinalIgnoreCase));
    }
}


