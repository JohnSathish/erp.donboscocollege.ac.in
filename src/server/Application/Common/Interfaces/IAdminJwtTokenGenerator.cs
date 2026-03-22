using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Common.Interfaces;

public interface IAdminJwtTokenGenerator
{
    JwtTokenResult GenerateToken(AdminUser adminUser, TimeSpan? lifetime = null);
}














