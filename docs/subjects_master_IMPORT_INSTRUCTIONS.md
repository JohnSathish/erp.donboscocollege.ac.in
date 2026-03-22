# How to fill `subjects_master_import_template.csv`

Use **Excel**, **Google Sheets**, or any editor. Save as **CSV UTF-8** (Excel: *Save As* → *CSV UTF-8 (Comma delimited)* on Windows).

## Columns

| Column | Allowed values | Required |
|--------|----------------|----------|
| `boardCode` | `MBOSE`, `CBSE`, `ISC` only | Yes |
| `streamCode` | `ARTS`, `SCIENCE`, `COMMERCE` | Yes |
| `subjectName` | Exact label shown to students (e.g. `Political Science`) | Yes |
| `sortOrder` | Integer; **lower = higher in list** (use 10, 20, 30 … to allow inserts) | Yes |

Do **not** put `OTHER` in this file — “Other” board uses free-text subjects in the form, not the master table.

## Rules

1. **One row per subject** for each **board + stream** where that subject should appear in the dropdown.
2. If the same subject exists for multiple streams (e.g. English), add **one row per stream** with the same `subjectName` if appropriate.
3. Remove the example rows and replace with your full lists.
4. Do not leave blank cells in the four columns.

## After you return the file

We will validate codes, remove duplicates, and import into `admissions.subjects_master` (or provide a seed script).
