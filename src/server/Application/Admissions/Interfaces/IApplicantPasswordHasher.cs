using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IApplicantPasswordHasher
{
    string HashPassword(StudentApplicantAccount account, string password);

    bool VerifyPassword(StudentApplicantAccount account, string hashedPassword, string providedPassword);
}

