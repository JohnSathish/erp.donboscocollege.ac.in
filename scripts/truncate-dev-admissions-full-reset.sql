-- =============================================================================
-- DEVELOPMENT ONLY — full admission pipeline + student records reset
-- =============================================================================
-- What this does:
--   1) TRUNCATE students."Students" CASCADE — removes all student rows and any
--      tables that reference them (guardians, course enrollments, fee rows,
--      attendance, exam marks, etc.). Use only on a dev database.
--   2) TRUNCATE all admissions.* tables listed below (except AdminUsers by default).
--   3) Re-inserts the singleton AdmissionWorkflowSettings row (Id = 1).
--
-- What is preserved (default):
--   • admissions."AdminUsers" — so you can still sign in to the admin API.
--   • admissions.subjects_master — Class XII catalog (comment out from TRUNCATE
--     if you want to wipe it too).
--
-- Run (example):
--   psql -h localhost -p 5432 -U postgres -d erp_dev -f scripts/truncate-dev-admissions-full-reset.sql
-- =============================================================================

BEGIN;

-- Step 1: clear enrolled students (FKs to admissions.StudentApplicantAccounts)
TRUNCATE TABLE students."Students" RESTART IDENTITY CASCADE;

-- Step 2: clear admissions operational + catalog data
TRUNCATE TABLE
    admissions."FeeComponents",
    admissions."FeeStructures",
    admissions."Courses",
    admissions."EntranceExams",
    admissions."ExamRegistrations",
    admissions."AdmissionOffers",
    admissions."MeritScores",
    admissions."OfflineFormIssuances",
    admissions."ApplicantApplicationDrafts",
    admissions."ApplicantRefreshTokens",
    admissions."Applicants",
    admissions."StudentApplicantAccounts",
    admissions."Programs",
    admissions.subjects_master,
    admissions."AdmissionWorkflowSettings"
RESTART IDENTITY CASCADE;

-- Step 3: singleton workflow settings (required for merit / direct-admission logic)
INSERT INTO admissions."AdmissionWorkflowSettings" ("Id", "MeritClassXiiCutoffPercentage", "UpdatedOnUtc", "UpdatedBy")
VALUES (1, 60.00, NOW() AT TIME ZONE 'utc', 'dev-reset-script');

COMMIT;

-- Optional verification (uncomment to run)
-- SELECT 'Admissions.StudentApplicantAccounts' AS t, COUNT(*) FROM admissions."StudentApplicantAccounts"
-- UNION ALL SELECT 'Students', COUNT(*) FROM students."Students";
