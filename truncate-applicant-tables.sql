-- SQL script to truncate all applicant-related tables
-- Run this script in your PostgreSQL database to clear all applicant data for testing

-- Disable foreign key constraints temporarily (PostgreSQL doesn't support this directly, so we'll delete in order)
-- Delete in order to respect foreign key relationships

-- 1. Delete refresh tokens (child table)
DELETE FROM admissions."ApplicantRefreshTokens";

-- 2. Delete application drafts (child table)
DELETE FROM admissions."ApplicantApplicationDrafts";

-- 3. Delete student applicant accounts (parent table with unique constraints)
DELETE FROM admissions."StudentApplicantAccounts";

-- 4. Delete applicants (if exists and has data)
DELETE FROM admissions."Applicants";

-- Reset sequences if any (PostgreSQL uses sequences for auto-increment, but these tables use UUIDs)
-- No sequences to reset for UUID-based tables

-- Verify tables are empty (optional - uncomment to check)
-- SELECT COUNT(*) FROM admissions."ApplicantRefreshTokens";
-- SELECT COUNT(*) FROM admissions."ApplicantApplicationDrafts";
-- SELECT COUNT(*) FROM admissions."StudentApplicantAccounts";
-- SELECT COUNT(*) FROM admissions."Applicants";

