-- Reset password for applicant DBCT25-0001
-- This script sets a temporary password "12345" for testing
-- Replace with actual password hash generation in production

-- First, check if the account exists
SELECT "Id", "UniqueId", "FullName", "Email", "PasswordHash", "MustChangePassword"
FROM admissions."StudentApplicantAccounts"
WHERE "UniqueId" = 'DBCT25-0001';

-- Note: The password hash needs to be generated using the same hasher as the application
-- For testing, you can use the reset password endpoint via Swagger UI instead
-- Or use the API endpoint: POST /api/admissions/applicants/DBCT25-0001/reset-password




