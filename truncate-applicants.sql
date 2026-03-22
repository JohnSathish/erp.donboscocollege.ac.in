-- Legacy shortcut: this file used to delete only applicant rows.
-- For a full dev reset (students + all admissions data), use:
--   scripts/truncate-dev-admissions-full-reset.sql

-- Minimal applicant-only delete (no student TRUNCATE) — kept for reference:
SET session_replication_role = 'replica';
DELETE FROM admissions."ApplicantApplicationDrafts";
DELETE FROM admissions."ApplicantRefreshTokens";
DELETE FROM admissions."StudentApplicantAccounts";
DELETE FROM admissions."Applicants";
SET session_replication_role = 'origin';
