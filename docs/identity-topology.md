# Identity Topology & Permission Model

## Overview
The ERP uses ASP.NET Core Identity integrated with Duende IdentityServer to provide OAuth2/OIDC services for all internal and external clients. Identity services are hosted in `src/server/Identity` and exposed as a dedicated WebAPI + IdentityServer host.

## Deployment Topology
- Single IdentityServer instance per environment (Dev, QA, Prod) fronted by HTTPS and reverse proxy (Azure App Gateway or Nginx).
- Backed by PostgreSQL schemas:
  - `identity`: ASP.NET Core Identity tables (users, roles, claims, persisted grants).
  - `configuration`: IdentityServer clients, scopes, API resources configuration (managed through EF Core migrations or seeding).
- Data protection keys stored in Redis (dev) and Azure Key Vault (prod) for token signing consistency across horizontal scale.
- Hangfire server co-located for scheduled identity tasks (e.g., cleanup, verification reminders).

## Client Applications
- **Web Portal (Angular)**
  - SPA using authorization code flow with PKCE.
  - Supports Students, Staff, Parents, Admin; post-login, role-based dashboard.
  - Refresh tokens with sliding expiration (configurable per client).
- **Mobile Companion (future)**
  - Native clients leveraging same authorization code flow (PKCE).
  - Device registration claims for push notification targeting.
- **Machine-to-Machine Integrations**
  - Confidential clients for back-office services (e.g., integrations, reporting pipelines) using client credentials flow.
- **Public Kiosks**
  - Optional device code flow with limited scope access.

## API Resources & Scopes
- `erp.api`: primary API resource exposing scopes aligned to modules:
  - `erp.api.academics`
  - `erp.api.students`
  - `erp.api.staff`
  - `erp.api.attendance`
  - `erp.api.fees`
  - `erp.api.payroll`
  - `erp.api.inventory`
  - `erp.api.markentry`
  - `erp.api.certification`
  - `erp.api.admissions`
  - `erp.api.notifications`
- `erp.sync`: M2M scope for data synchronization jobs.
- `offline_access`: enabled for refresh tokens.

Scopes are mapped to policy names in the API layer to enforce module access.

## Permission Model
Permissions are a combination of roles and fine-grained policies stored in the database:

| Role | Description | Default Scopes | Key Policies |
|------|-------------|----------------|--------------|
| `SystemAdministrator` | Full platform control | All module scopes | `CanManageTenants`, `CanManageIdentity` |
| `AcademicAdmin` | Manages academics module | `erp.api.academics`, `erp.api.students` | `CanPublishTimetable`, `CanApproveMarks` |
| `FinanceOfficer` | Manages billing | `erp.api.fees`, `erp.api.payroll` | `CanIssueRefunds`, `CanViewPayroll` |
| `HRManager` | Staff lifecycle owner | `erp.api.staff`, `erp.api.payroll` | `CanHireStaff`, `CanRunPayroll` |
| `InventoryManager` | Oversees stock | `erp.api.inventory` | `CanAdjustStock`, `CanApprovePO` |
| `AdmissionsOfficer` | Admission workflows | `erp.api.admissions`, `erp.api.students` | `CanIssueOffer`, `CanConvertApplicant` |
| `Teacher` | Academic staff | `erp.api.academics`, `erp.api.markentry`, `erp.api.attendance` | `CanEnterMarks`, `CanTakeAttendance` |
| `Student` | Self-service access | `erp.api.students` (limited), `erp.api.academics` (read-only) | `CanViewOwnProfile`, `CanViewSchedule` |
| `Parent` | Guardian portal | `erp.api.students` (read-only child data) | `CanViewFees`, `CanViewAttendance` |

### Policy Patterns
- Policy records stored in `Permission` table referencing module, action, and optional scope.
- `UserPermissionsService` resolves effective permissions at login, caching in Redis.
- Angular client receives permission matrix via `/me` endpoint; client-side guards enforce navigation authorization.

## Identity Lifecycle
1. **Provisioning**
   - Admissions module triggers student and guardian accounts when status = enrolled.
   - Staff module provisions staff accounts upon onboarding approval.
   - Admin roles assigned by System Administrator via administrative UI.
2. **Authentication**
   - Username/email + password with optional MFA (app-based TOTP).
   - External login providers (e.g., Azure AD) pluggable for staff.
3. **Authorization**
   - Role claims assigned at login.
   - Module permissions loaded as custom claims (`permission:<module>:<action>`).
   - API uses policy-based authorization referencing these claims.
4. **Governance**
   - Password policies, lockouts, and 2FA enforcement configurable per tenant.
   - Access reviews and automated deactivation based on HR/Admissions statuses.
   - Audit logs stored for login events, consent, admin actions.

## Operational Considerations
- Use IdentityServer's dynamic client configuration for campus-specific portals.
- Apply rate limiting on token endpoints.
- Rotate signing certificates regularly and automate via Key Vault.
- Implement SCIM-compatible provisioning endpoint for future HR integrations.




