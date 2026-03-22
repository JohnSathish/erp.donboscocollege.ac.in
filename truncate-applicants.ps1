# PowerShell script to truncate applicant account data
# This will delete all registered applicant accounts

$env:PGPASSWORD = "john@1991js"

psql -h localhost -p 5432 -U postgres -d erp_dev -c @"
-- Delete in order to respect foreign key constraints
DELETE FROM admissions."ApplicantApplicationDrafts";
DELETE FROM admissions."ApplicantRefreshTokens";
DELETE FROM admissions."StudentApplicantAccounts";
DELETE FROM admissions."Applicants";

-- Verify tables are empty
SELECT 'ApplicantApplicationDrafts' as table_name, COUNT(*) as count FROM admissions."ApplicantApplicationDrafts"
UNION ALL
SELECT 'ApplicantRefreshTokens', COUNT(*) FROM admissions."ApplicantRefreshTokens"
UNION ALL
SELECT 'StudentApplicantAccounts', COUNT(*) FROM admissions."StudentApplicantAccounts"
UNION ALL
SELECT 'Applicants', COUNT(*) FROM admissions."Applicants";
"@

$env:PGPASSWORD = ""




