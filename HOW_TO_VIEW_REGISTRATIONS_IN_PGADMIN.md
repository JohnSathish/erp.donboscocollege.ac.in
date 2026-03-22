# How to View Registration Details in pgAdmin

## Database Connection Details

- **Database Name**: `erp_dev`
- **Schema**: `admissions`
- **Main Table**: `StudentApplicantAccounts`

## Step-by-Step Guide

### Step 1: Connect to Database in pgAdmin

1. Open **pgAdmin**
2. Expand **Servers** → **PostgreSQL** → **Databases**
3. Right-click on **`erp_dev`** → **Query Tool**

### Step 2: View All Registrations

Run this query to see all registered students:

```sql
SELECT 
    "UniqueId" AS "Application Number",
    "FullName" AS "Full Name",
    "Email",
    "MobileNumber" AS "Mobile Number",
    "DateOfBirth" AS "Date of Birth",
    "Gender",
    "Shift",
    "CreatedOnUtc" AS "Registration Date",
    "MustChangePassword" AS "Password Changed"
FROM admissions."StudentApplicantAccounts"
ORDER BY "CreatedOnUtc" DESC;
```

### Step 3: View Specific Registration by Email

```sql
SELECT 
    "UniqueId" AS "Application Number",
    "FullName" AS "Full Name",
    "Email",
    "MobileNumber" AS "Mobile Number",
    "DateOfBirth" AS "Date of Birth",
    "Gender",
    "Shift",
    "PhotoUrl" AS "Photo URL",
    "CreatedOnUtc" AS "Registration Date",
    "MustChangePassword" AS "Password Changed"
FROM admissions."StudentApplicantAccounts"
WHERE "Email" = 'student@example.com';
```

### Step 4: View Registration by Application Number (UniqueId)

```sql
SELECT 
    "UniqueId" AS "Application Number",
    "FullName" AS "Full Name",
    "Email",
    "MobileNumber" AS "Mobile Number",
    "DateOfBirth" AS "Date of Birth",
    "Gender",
    "Shift",
    "PhotoUrl" AS "Photo URL",
    "CreatedOnUtc" AS "Registration Date",
    "MustChangePassword" AS "Password Changed"
FROM admissions."StudentApplicantAccounts"
WHERE "UniqueId" = 'APP001';
```

### Step 5: View Recent Registrations (Last 24 Hours)

```sql
SELECT 
    "UniqueId" AS "Application Number",
    "FullName" AS "Full Name",
    "Email",
    "MobileNumber" AS "Mobile Number",
    "DateOfBirth" AS "Date of Birth",
    "Gender",
    "CreatedOnUtc" AS "Registration Date"
FROM admissions."StudentApplicantAccounts"
WHERE "CreatedOnUtc" >= NOW() - INTERVAL '24 hours'
ORDER BY "CreatedOnUtc" DESC;
```

### Step 6: Count Total Registrations

```sql
SELECT COUNT(*) AS "Total Registrations"
FROM admissions."StudentApplicantAccounts";
```

### Step 7: View Registrations by Date Range

```sql
SELECT 
    "UniqueId" AS "Application Number",
    "FullName" AS "Full Name",
    "Email",
    "MobileNumber" AS "Mobile Number",
    "CreatedOnUtc" AS "Registration Date"
FROM admissions."StudentApplicantAccounts"
WHERE "CreatedOnUtc" >= '2024-01-01'
  AND "CreatedOnUtc" < '2024-12-31'
ORDER BY "CreatedOnUtc" DESC;
```

### Step 8: View Registrations by Gender

```sql
SELECT 
    "Gender",
    COUNT(*) AS "Count"
FROM admissions."StudentApplicantAccounts"
GROUP BY "Gender"
ORDER BY "Count" DESC;
```

### Step 9: View Registrations with Photo

```sql
SELECT 
    "UniqueId" AS "Application Number",
    "FullName" AS "Full Name",
    "Email",
    "PhotoUrl" AS "Photo URL"
FROM admissions."StudentApplicantAccounts"
WHERE "PhotoUrl" IS NOT NULL;
```

### Step 10: View All Columns (Complete Details)

```sql
SELECT *
FROM admissions."StudentApplicantAccounts"
ORDER BY "CreatedOnUtc" DESC
LIMIT 10;
```

## Table Structure

The `StudentApplicantAccounts` table contains:

| Column | Type | Description |
|--------|------|-------------|
| `Id` | uuid | Primary key (GUID) |
| `UniqueId` | varchar(32) | Application number (unique) |
| `FullName` | varchar(256) | Student's full name |
| `DateOfBirth` | date | Date of birth |
| `Gender` | varchar(32) | Gender |
| `Email` | varchar(256) | Email address (unique) |
| `MobileNumber` | varchar(20) | Mobile number (unique) |
| `Shift` | varchar(64) | Shift selection |
| `PasswordHash` | varchar(512) | Hashed password |
| `PhotoUrl` | varchar(512) | Profile photo URL (nullable) |
| `CreatedOnUtc` | timestamp | Registration timestamp |
| `MustChangePassword` | boolean | Password change required flag |

## Quick Tips

1. **Always use double quotes** for table and column names in PostgreSQL
2. **Schema name is required**: Use `admissions."StudentApplicantAccounts"`
3. **Case-sensitive**: Column names are case-sensitive
4. **Use LIMIT** for large result sets: `LIMIT 100`

## Common Queries

### Find Duplicate Emails (should be none due to unique constraint)
```sql
SELECT "Email", COUNT(*) AS "Count"
FROM admissions."StudentApplicantAccounts"
GROUP BY "Email"
HAVING COUNT(*) > 1;
```

### Find Registrations Without Photos
```sql
SELECT 
    "UniqueId",
    "FullName",
    "Email"
FROM admissions."StudentApplicantAccounts"
WHERE "PhotoUrl" IS NULL;
```

### Export to CSV (in pgAdmin)
1. Run your query
2. Right-click on results → **Download as CSV**

## Security Note

⚠️ **Never share or expose password hashes** - they are stored securely and should not be displayed in queries for security reasons.



















