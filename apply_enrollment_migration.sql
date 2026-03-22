-- Migration: Add Enrollment Fields to StudentApplicantAccounts
-- Migration Name: AddEnrollmentFields
-- Date: 2025-11-20

-- Add EnrolledOnUtc column
ALTER TABLE admissions."StudentApplicantAccounts" 
ADD COLUMN IF NOT EXISTS "EnrolledOnUtc" timestamp with time zone NULL;

-- Add EnrollmentRemarks column
ALTER TABLE admissions."StudentApplicantAccounts" 
ADD COLUMN IF NOT EXISTS "EnrollmentRemarks" character varying(1000) NULL;

-- Verify columns were added
SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_schema = 'admissions' 
  AND table_name = 'StudentApplicantAccounts'
  AND column_name IN ('EnrolledOnUtc', 'EnrollmentRemarks');









