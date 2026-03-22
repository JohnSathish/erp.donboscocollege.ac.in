# Wiring the applicant portal — Class XII (Academic Records) step

The **ERP API** already exposes draft save/load and subject catalog endpoints. The **multi-step applicant UI** (screenshot you used earlier) is expected to live in a **separate front-end project** (Angular/React/etc.). This doc describes how to connect that UI to the API.

---

## 1. APIs to use

| Purpose | Method | URL |
|--------|--------|-----|
| Load/save draft (includes academics) | `GET` / `POST` | `/api/applicant-applications/me` |
| **Subject dropdown** (MBOSE/CBSE/ISC only) | `GET` | `/api/admissions/class-xii-subjects?board={board}&stream={stream}` |
| No auth on subject list | — | `AllowAnonymous` |

**Board query values:** `MBOSE`, `CBSE`, `ISC` (not `OTHER`).  
**Stream query values:** `ARTS`, `SCIENCE`, `COMMERCE`.

**GET subjects response:**

```json
{
  "items": [
    { "id": "uuid", "subjectName": "English", "sortOrder": 10 }
  ]
}
```

Use **`id`** as **`subjectMasterId`** in the draft when the row was chosen from the dropdown; use **`subjectName`** for display and for the stored **`subject`** string.

---

## 2. Draft JSON shape (`academics`)

When saving the draft (`POST /api/applicant-applications/me`), send **`academics`** like this (camelCase in JSON):

```json
{
  "academics": {
    "classXiiBoardCode": "MBOSE",
    "classXiiStreamCode": "ARTS",
    "classXiiSubjects": [
      {
        "subject": "English",
        "marks": "85",
        "subjectMasterId": "optional-guid-from-items.id",
        "entryMode": "master"
      },
      {
        "subject": "Education",
        "marks": "78",
        "subjectMasterId": "…",
        "entryMode": "master"
      }
    ],
    "classXII": [],
    "boardExamination": { ... },
    "cuet": { ... },
    "lastInstitutionAttended": "..."
  }
}
```

**Rules:**

- **`classXiiBoardCode` / `classXiiStreamCode`:** use the same codes as the GET query (`MBOSE`, `OTHER`, etc.).
- **`OTHER` board:** do **not** call the subjects GET; use **manual** rows only:
  - `entryMode`: `"manual"`
  - `subjectMasterId`: omit or `null`
  - `subject`: free text from the user
- **Controlled boards:** after the user picks a subject from the dropdown, set:
  - `subject` = selected **`subjectName`**
  - `subjectMasterId` = selected **`id`**
  - `entryMode` = `"master"`
- The API **syncs** `classXiiSubjects` → legacy **`classXII`** on save; you can focus on **`classXiiSubjects`** in the UI. Keeping **`classXII`** empty on send is OK if the server normalizes (this project does).

---

## 3. UI flow (recommended)

1. **Board** dropdown: `MBOSE` | `CBSE` | `ISC` | `OTHER`.
2. **Stream** dropdown: `ARTS` | `SCIENCE` | `COMMERCE` (hide or disable if board is `OTHER`, or still show if your policy requires stream for analytics — for “Other”, stream can be stored or left empty per product rule).
3. When **board ≠ OTHER** and both board + stream are chosen:
   - `GET /api/admissions/class-xii-subjects?board=...&stream=...`
   - Build one dropdown option per `items[]` (sort by `sortOrder` then label).
4. **Dynamic rows** (default 5): each row = subject control + marks + remove.
   - **Controlled:** `<select>` options from `items`; disable options already picked in other rows.
   - **OTHER:** `<input type="text">` for subject name.
5. **Marks:** validate numeric, 0–100 (client-side + trust server rules later).
6. **Minimum 5** filled rows when using the new flow (backend completion rule when both `classXiiBoardCode` and `classXiiStreamCode` are set).

---

## 4. Angular example (sketch)

```typescript
// environment
readonly apiBase = 'http://localhost:5227';

// load subjects when board/stream change (skip OTHER)
loadSubjects() {
  const b = this.form.classXiiBoardCode;
  const s = this.form.classXiiStreamCode;
  if (b === 'OTHER' || !b || !s) {
    this.subjectOptions = [];
    return;
  }
  this.http.get<{ items: { id: string; subjectName: string; sortOrder: number }[] }>(
    `${this.apiBase}/api/admissions/class-xii-subjects`,
    { params: { board: b, stream: s } }
  ).subscribe(r => {
    this.subjectOptions = [...r.items].sort((a, b) => a.sortOrder - b.sortOrder);
  });
}

onSubjectSelected(rowIndex: number, itemId: string) {
  const item = this.subjectOptions.find(x => x.id === itemId);
  const row = this.rows[rowIndex];
  row.subject = item?.subjectName ?? '';
  row.subjectMasterId = item?.id;
  row.entryMode = 'master';
}
```

Persist **`rows`** into **`academics.classXiiSubjects`** when calling your existing **save draft** service.

---

## 5. Auth

- **Subject list:** no bearer token required.
- **Draft `GET`/`POST`:** applicant **JWT** (`Authorization: Bearer …`) as you already use for the portal.

---

## 6. If your portal is not in this repo

1. Open the project that contains the **“Academic Records”** / **Class XII** step component.
2. Find where it builds **`academics`** (or the old `classXII` array only).
3. Replace static “Subject 1…8” fields with:
   - board + stream controls,
   - dynamic rows,
   - `GET` subjects for non-OTHER,
   - payload as in section 2.

---

## 7. Verify end-to-end

1. Save draft with new fields → `GET /api/applicant-applications/me` → confirm `classXiiSubjects` and synced `classXII`.
2. Open Swagger → optional: inspect DB table `admissions.subjects_master` if needed.

If you **add the applicant portal source** into this workspace (or paste the component path), the same wiring can be applied directly in code here.
