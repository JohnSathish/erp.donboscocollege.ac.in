# Admission module (boundary)

The **admission** workflow is isolated behind the `admissions` API and `StudentApplicantAccount` aggregate. ERP **student** records live in `students.Students` and are linked **after approval** via `AdmissionErpSync`.

## KPIs (`GET /api/admissions/admin/dashboard`)

| Metric | Source field |
|--------|----------------|
| Total applications | `submittedApplications` |
| Paid applications | `paidApplications` |
| Pending review | `pendingPipelineCount` (submitted, not approved/rejected/enrolled) |
| Approved | `approvedApplications` |
| Rejected | `rejectedApplications` |
| Offline forms issued | `offlineFormsIssued` (`AdmissionChannel == Offline`) |
| Offline forms received | `offlineFormsReceived` (`OfflineFormReceivedOnUtc` set) |
| Offline submitted | `offlineApplicationsSubmitted` |

## Offline admission

- **Issue form (admin):** `POST /api/admissions/admin/offline-forms/issue` — stores **only** `admissions.OfflineFormIssuances` (form #, name, mobile, fee, issued date); **no** `StudentApplicantAccount` yet; PDF receipt (**no subject / mobile on slip**; half A4) via `Admissions:ReceiptBranding` + optional `Logo.png`.
- **Preview (admin):** `GET /api/admissions/admin/offline-forms/{formNumber}/preview` — issued slip (pending) or existing offline applicant.
- **Receive form:** `POST /api/admissions/admin/offline-forms/receive` — body `{ formNumber, majorSubject }`; **creates** `StudentApplicantAccount` from the issuance, sets `OfflineIssuedMajorSubject`, `OfflineFormReceivedOnUtc`, links issuance; legacy offline-only accounts (no issuance row) still supported for marking received.
- **Receipt reprint:** `GET /api/admissions/admin/offline-forms/{formNumber}/receipt` (admin JWT).
- **Applicant login:** username = form number, password = mobile (first login); `MustChangePassword` applies.
- **Applications list filter:** `GET /api/admissions/applications?admissionChannel=offline|online`.
- **Selection list:** assign round `POST /api/admissions/applications/{id}/selection-list-round`, publish `POST /api/admissions/admin/selection-list/publish` (optional SMS).
- **Public list:** `GET /api/admissions/public/selection-list?round=First|Second|Third` (anonymous).
- **Staff UI (dev):** applicant-portal route `/offline-admission` (admin JWT in session).

## ERP sync

- **Automatic:** When status transitions to **Approved**, `IAdmissionErpSyncService` provisions a `Student` (or links an existing one) and stores `ErpStudentId` on the applicant account.
- **Manual:** `POST /api/admissions/applications/{id}/erp-sync` — idempotent retry.
- **Configuration:** `AdmissionErpSync` in `appsettings.json` (`AcademicYear`, optional `DefaultProgramId` / `DefaultProgramCode`, `Enabled`).

## Database

Apply migration `AddAdmissionErpSyncFields` (columns `ErpStudentId`, `ErpSyncedOnUtc`, `ErpSyncLastError` on `admissions.StudentApplicantAccounts`).

Apply migration `AddOfflineAdmissionModule` (offline admission + selection list columns on `admissions.StudentApplicantAccounts`).

```bash
dotnet ef database update --project src/server/Infrastructure/ERP.Infrastructure.csproj --startup-project src/server/Api/ERP.Api.csproj
```

## Frontend

- **Admission Control Center** (`/admin/dashboard`): TanStack Table (`@tanstack/angular-table`) drives the applications grid; sorting stays **server-side**.
- Excel export uses the existing server export; swapping to **ExcelJS** or PDF to **Puppeteer** can be done inside `ApplicantExportService` / `ApplicantApplicationPdfService` without changing route contracts.
