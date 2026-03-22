using MediatR;

namespace ERP.Application.Admissions.Events;

public sealed record StudentApplicantRegisteredEvent(
    Guid AccountId,
    string UniqueId,
    string FullName,
    string Email,
    string MobileNumber,
    string TemporaryPassword) : INotification;

