namespace ERP.Application.Admissions.Interfaces;

public interface IAdmissionFeeService
{
    /// <summary>
    /// Gets the admission fee amount for a given stream/major subject.
    /// </summary>
    /// <param name="majorSubject">The major subject/stream (e.g., "Science", "Arts", "Commerce")</param>
    /// <returns>The admission fee amount in rupees</returns>
    decimal GetAdmissionFee(string? majorSubject);

    /// <summary>
    /// Validates if the paid amount matches the required admission fee.
    /// </summary>
    /// <param name="paidAmount">The amount that was paid</param>
    /// <param name="majorSubject">The major subject/stream</param>
    /// <returns>True if the paid amount matches or exceeds the required fee</returns>
    bool IsPaymentAmountValid(decimal? paidAmount, string? majorSubject);
}


