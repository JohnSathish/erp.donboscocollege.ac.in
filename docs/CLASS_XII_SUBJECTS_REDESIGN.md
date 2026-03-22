# Class XII Subjects & Marks — Redesign (Design Spec)

This document describes the target architecture, data model, APIs, UI behavior, and validation rules. **Subject master rows are loaded from your spreadsheet later** using the template in `subjects_master_import_template.csv`.

---

## 1. Goals (summary)

| Goal | Approach |
|------|----------|
| Board + Stream drive dropdowns | Canonical codes stored in DB; UI loads options via API |
| “Other” board = free text | Same row model; `entryMode = manual`; no `subjectMasterId` |
| Dynamic rows | `subjects[]` array; default 5 rows; add/remove |
| Minimum 5 subjects | Client + server validation |
| No duplicate subjects (dropdown mode) | Validate on change + on submit |
| Compact table + mobile stack | Responsive table / card rows |
| Accurate admission processing | Single JSON shape for draft, export, and PDF |

---

## 2. Canonical codes (recommended)

Use **stable English codes** in API and JSON (labels can be localized in UI).

**Board** (`boardCode`):

| Code | Label (UI) |
|------|------------|
| `MBOSE` | MBOSE |
| `CBSE` | CBSE |
| `ISC` | ISC |
| `OTHER` | Other |

**Stream** (`streamCode`):

| Code | Label (UI) |
|------|------------|
| `ARTS` | Arts |
| `SCIENCE` | Science |
| `COMMERCE` | Commerce |

`BoardExamination.BoardName` in existing drafts can be **migrated** by mapping common strings (e.g. “Meghalaya Board …” → `MBOSE`) or left as display-only; the **authoritative** board for subject selection should be `boardCode` on the academic section (see §3).

---

## 3. Target JSON shape (draft `academics`)

Extend **`AcademicSection`** (backward-compatible migration):

```json
{
  "classXiiBoardCode": "CBSE",
  "classXiiStreamCode": "SCIENCE",
  "classXiiSubjects": [
    { "subject": "Physics", "marks": "85", "subjectMasterId": "a1b2c3d4-...", "entryMode": "master" },
    { "subject": "Chemistry", "marks": "78", "subjectMasterId": "...", "entryMode": "master" }
  ],
  "boardExamination": { ... },
  "cuet": { ... },
  "lastInstitutionAttended": "..."
}
```

**Field notes:**

- **`classXiiBoardCode` / `classXiiStreamCode`**: Required before subject rows are meaningful (except you may allow saving partial drafts).
- **`classXiiSubjects`**: Replaces the old **fixed “Subject 1–8”** list. Legacy `classXII` (same idea, different name) can be **read** for old applications and **written** as `classXiiSubjects` on next save.
- **`subject`**: Always the display string stored for export/PDF (for master mode = chosen label from master; for manual = user typed).
- **`subjectMasterId`**: Optional `Guid`; set when row selected from dropdown; omitted/null for “Other” or manual.
- **`entryMode`**: `"master"` | `"manual"` — derived from board: `OTHER` → always manual; else master when using dropdown.

**Backward compatibility:**

- If `classXiiSubjects` is empty but legacy `classXII` has items, treat legacy as manual rows with `entryMode: "manual"` until user picks board/stream again.
- Percentage/total logic continues to use existing parsers from `BoardExamination` / marks as today.

---

## 4. Database: `admissions.subjects_master`

| Column | Type | Notes |
|--------|------|--------|
| `Id` | `uuid` PK | |
| `BoardCode` | `varchar(16)` | `MBOSE`, `CBSE`, `ISC` (not `OTHER`) |
| `StreamCode` | `varchar(16)` | `ARTS`, `SCIENCE`, `COMMERCE` |
| `SubjectName` | `varchar(256)` | Exact label shown in dropdown |
| `SortOrder` | `int` | Default 0; order within board+stream |
| `IsActive` | `bool` | Soft-disable without deleting |
| `CreatedOnUtc` | `timestamptz` | |

**Indexes:** unique `(BoardCode, StreamCode, SubjectName)` (case-insensitive optional) + index on `(BoardCode, StreamCode, IsActive)`.

**Seeding:** from CSV/Excel import (admin tool or one-off SQL) using your filled template.

---

## 5. API

### 5.1 Public / applicant reference data

`GET /api/admissions/class-xii-subjects?board=CBSE&stream=SCIENCE`

- **Auth:** same as other applicant-facing reference endpoints (e.g. `AllowAnonymous` read-only or `Applicant` role — align with your security policy).
- **Query:** `board`, `stream` — required; reject `OTHER` with `400` or return empty list (subjects are free-text only for Other).
- **Response:**

```json
{
  "items": [
    { "id": "...", "subjectName": "Physics", "sortOrder": 10 }
  ]
}
```

### 5.2 Admin (optional, later)

- `GET/POST/PATCH` for CRUD or CSV import — not required for first applicant rollout if you import via SQL.

---

## 6. UI flow

### Step A — Board & Stream

- Dropdowns: Board (MBOSE, CBSE, ISC, Other), Stream (Arts, Science, Commerce).
- On change: reset subject rows **only if** you confirm data loss, or keep marks and clear invalid selections (product choice).

### Step B — Subject rows

**Controlled (MBOSE / CBSE / ISC):**

- Table: **Subject** | **Marks** | **Remove**
- Subject cell: `<select>` populated from API; options already selected in other rows **disabled** or filtered out.
- Duplicate attempt: inline error on that row.

**Manual (Other):**

- Same layout; Subject cell: `<input type="text">` with helper: *“Please enter subject names exactly as per your marksheet.”*

**Rows:**

- Initial load: **5 rows** (empty selects/inputs + marks).
- **+ Add subject** appends a row.
- **Remove** allowed if row count > 5? **Spec says minimum 5 subjects:** allow remove only if remaining ≥ 5, or allow remove with submit blocked until 5 filled (clearer UX: **cannot remove below 5 rows**).

**Sections:**

- **Core subjects (minimum 5)** — first five rows visually, or single table with note.
- **Additional subjects (optional)** — rows 6+.

### Mobile

- Each row becomes a **card**: Subject control full width, then Marks full width, then Remove (full width button or icon with tap target ≥ 44px).

---

## 7. Validations

| Rule | Client | Server (on save/submit) |
|------|--------|---------------------------|
| Board & stream selected | ✓ | ✓ |
| ≥ 5 rows with non-empty subject + marks | ✓ | ✓ |
| Marks numeric, 0–100 | ✓ | ✓ |
| No duplicate `subject` (trim, case-insensitive) in master mode | ✓ | ✓ |
| Selected subject belongs to board+stream (master) | ✓ (implicit) | ✓ (optional: resolve `subjectMasterId`) |
| Other board: free text, no duplicate check required? | Recommend **same** duplicate rule on normalized text | ✓ |

Server-side validation should live in a **single validator** used by `SaveApplicantApplicationDraft` and `SubmitApplicantApplication` so the portal cannot bypass rules.

---

## 8. PDF / export / admin

- **PDF:** Render `classXiiSubjects` as a table (subject + marks); board/stream in header.
- **Excel export:** Already iterates `ClassXII`; switch to new property and add columns for Board/Stream if needed.

---

## 9. Implementation phases (suggested)

1. **DB + migration** for `subjects_master`.
2. **GET subjects** endpoint + integration test.
3. **DTO + validator** updates + legacy draft migration in read path.
4. **Applicant UI** (your portal repo): board/stream + dynamic table.
5. **Import** your CSV into `subjects_master`.
6. **PDF/export** updates if column layout changes.

---

## 10. What we need from you (board × stream lists)

Please fill **`docs/subjects_master_import_template.csv`** (one row per subject). For each **board + stream** combination, include **every** subject a student might take in Class XII that you want in the dropdown.

**Questions to confirm:**

1. Should **Stream** apply to **MBOSE** the same way as CBSE (three streams), or does MBOSE use different stream labels?
2. For **ISC**, confirm stream split (Arts / Science / Commerce) matches your office records.
3. Any subject that appears under **more than one** stream (e.g. English) — list it on **each** row with the same name, or once per stream as appropriate.
4. Preferred **sort order** (e.g. English first, MIL second): use `sortOrder` column in the CSV.

Once the file is returned, we can generate seed SQL or an admin import.

---

## 11. Relation to current ERP repo

- Backend draft DTOs and PDF/export live in **`src/server`**; the **step-4 UI** in your screenshot may live in a **separate applicant portal** repository. This spec applies to both; wire the portal to the new API and JSON fields when implementing.
