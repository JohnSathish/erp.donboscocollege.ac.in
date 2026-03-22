# High-Level Data Model

This document outlines the conceptual entities and relationships that underpin the initial ERP modules. Detailed ERDs will evolve as modules mature, but this serves as a baseline for schema planning in PostgreSQL.

## Core Entities
- **Tenant**: Supports multi-campus capability. Key attributes: `TenantId`, `Name`, `Code`, `ContactInfo`, `Settings`.
- **User**: Identity record linked to ASP.NET Core Identity tables. Attributes: `UserId`, `TenantId`, `IdentityUserId`, `UserType` (Student, Staff, Parent, Admin), `Status`.
- **RoleAssignment**: Maps users to roles and modules. Attributes: `RoleAssignmentId`, `UserId`, `RoleId`, `Scope` (tenant/module), `EffectiveFrom/To`.

## Admissions & Student Lifecycle
- **Applicant**: Captures prospective student data. Attributes: demographics, prior education, program preferences.
- **ApplicationDocument**: Stores metadata for uploaded docs (file stored via blob storage). Attributes: `DocumentType`, `StorageUri`, `Status`.
- **AdmissionOffer**: Represents offers and acceptance. Attributes: `OfferId`, `ApplicantId`, `ProgramId`, `Status`, `OfferDate`.
- **Student**: Activated upon enrollment. Attributes: `StudentId`, `ApplicantId`, `EnrollmentDate`, `CurrentProgramId`, `AcademicStatus`.
- **Guardian**: Parent/guardian contact details. Relationships: `GuardianStudent` (many-to-many with role type).

## Academics
- **Program**: Degree/diploma programs. Attributes: `ProgramId`, `Name`, `Level`, `Duration`, `Credits`.
- **Course**: Subjects within programs. Attributes: `CourseId`, `Code`, `Title`, `CreditHours`, `ProgramId`.
- **Term**: Academic term/semester definitions. Attributes: `TermId`, `AcademicYear`, `StartDate`, `EndDate`.
- **ClassSection**: Specific offering of a course to a batch. Attributes: `SectionId`, `CourseId`, `TermId`, `TeacherId`, `Capacity`.
- **TimetableSlot**: Scheduled sessions. Attributes: `SlotId`, `SectionId`, `DayOfWeek`, `StartTime`, `Duration`, `RoomId`.

## Attendance
- **AttendanceSession**: Represents a session requiring attendance (linked to class section or staff shift).
- **AttendanceRecord**: Student/staff attendance entries. Attributes: `RecordId`, `SessionId`, `PersonId`, `Status`, `Timestamp`, `MarkedBy`.
- **AttendanceDeviceEvent**: Optional capture of biometric or RFID scans for reconciliation.

## Fees & Finance
- **FeeStructure**: Defines fee components per program/term. Attributes: `StructureId`, `ProgramId`, `TermId`, `Component`, `Amount`.
- **Invoice**: Student billing instance. Attributes: `InvoiceId`, `StudentId`, `DueDate`, `Status`, `Total`.
- **InvoiceLine**: Detailed charges. Attributes: `LineId`, `InvoiceId`, `Component`, `Amount`.
- **Payment**: Recorded payment transaction. Attributes: `PaymentId`, `InvoiceId`, `Gateway`, `Reference`, `Amount`, `Status`, `ReceivedOn`.
- **Scholarship**: Tuition support applied to invoices. Attributes: `ScholarshipId`, `StudentId`, `Percentage/Amount`, `EffectiveTerms`.

## Payroll & Staff
- **StaffMember**: Core staff profile. Attributes: `StaffId`, `EmployeeNumber`, `DepartmentId`, `Designation`, `JoinDate`, `Status`.
- **EmploymentContract**: Contract terms and salary structure. Attributes: `ContractId`, `StaffId`, `Type`, `StartDate`, `EndDate`, `Grade`, `CTC`.
- **PayrollCycle**: Payroll runs per period. Attributes: `CycleId`, `PeriodStart`, `PeriodEnd`, `Status`.
- **Payslip**: Generated payslip per staff. Attributes: `PayslipId`, `CycleId`, `StaffId`, `Gross`, `Deductions`, `Net`.
- **LeaveBalance**: Tracks leave types and balances impacting payroll.

## Inventory
- **InventoryItem**: SKU master. Attributes: `ItemId`, `Name`, `CategoryId`, `UnitOfMeasure`, `ReorderLevel`.
- **StockLedger**: Movement history. Attributes: `LedgerId`, `ItemId`, `TransactionType`, `Quantity`, `ReferenceType/Id`, `Timestamp`.
- **PurchaseOrder**: Procurement record. Attributes: `POId`, `SupplierId`, `Status`, `Total`.
- **GoodsReceipt**: Received goods linked to PO. Attributes: `GRNId`, `POId`, `ReceivedDate`, `ReceivedBy`.
- **Asset**: Trackable assets with depreciation schedule.

## Marks & Certification
- **Assessment**: Exam or evaluation definition. Attributes: `AssessmentId`, `CourseId`, `TermId`, `Type`, `Weightage`.
- **AssessmentComponent**: Sub-components (theory, practical). Attributes: `ComponentId`, `AssessmentId`, `MaxMarks`.
- **MarkEntry**: Student marks. Attributes: `MarkEntryId`, `AssessmentComponentId`, `StudentId`, `MarksObtained`, `Status`.
- **ResultSummary**: Consolidated results per term/student. Attributes: `SummaryId`, `StudentId`, `TermId`, `GPA`, `ResultStatus`.
- **CertificateRequest**: Student-initiated request. Attributes: `RequestId`, `StudentId`, `CertificateType`, `Status`.
- **CertificateIssue**: Issued certificate metadata. Attributes: `CertificateId`, `RequestId`, `IssueDate`, `DocumentUri`.

## Notifications
- **MessageTemplate**: Predefined notification formats. Attributes: `TemplateId`, `Channel`, `Body`, `Tokens`.
- **NotificationEvent**: Trigger instance. Attributes: `EventId`, `Module`, `EventType`, `Payload`.
- **NotificationDispatch**: Records outbound messages. Attributes: `DispatchId`, `EventId`, `Recipient`, `Channel`, `Status`.

## Relationships Overview
```
Tenant 1---* Program 1---* Course 1---* ClassSection *---1 StaffMember
Tenant 1---* Student *---* Guardian
Student 1---* Invoice 1---* InvoiceLine
Invoice 1---* Payment
Student 1---* AttendanceRecord *---1 AttendanceSession
StaffMember 1---* Payslip *---1 PayrollCycle
InventoryItem 1---* StockLedger
Assessment 1---* AssessmentComponent 1---* MarkEntry *---1 Student
CertificateRequest 1---1 CertificateIssue
```

## Data Governance
- Row-level security per tenant enforced at the database level using PostgreSQL RLS policies.
- Soft deletes via `IsDeleted` flag on sensitive entities (`Student`, `StaffMember`, `Invoice`) with audit logging.
- Time-stamped audit tables via EF Core interceptors (`CreatedOn`, `ModifiedOn`, `CreatedBy`, `ModifiedBy`).
- Sensitive data encryption at rest for personally identifiable information using PostgreSQL pgcrypto or application-level encryption.




