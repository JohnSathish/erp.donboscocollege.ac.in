# Domain Boundaries & Sub-Features

This document captures the high-level scope and responsibilities for each ERP module. Each boundary is intended to translate into a dedicated feature slice in the `.NET` backend (`src/server/<Module>`) and a corresponding Angular library (`src/client/<module>`).

## Admissions
- Online application intake and validation workflows
- Document uploads and verification tracking
- Application fee payments and reconciliation
- Admission ranking, offers, and acceptance tracking
- Automated creation of student/parent identities post-enrollment

## Students
- Student profile lifecycle (enrollment, transfers, exit)
- Academic history and progression
- Discipline and counseling records
- Parent/guardian associations
- Portal dashboard, announcements, and notifications

## Staff
- Staff recruitment and onboarding workflows
- Contract and employment record management
- Qualifications, trainings, and certifications
- Performance reviews and probation tracking
- Staff portal dashboard and internal communications

## Academics
- Program, course, and curriculum management
- Academic calendar and timetable generation
- Section/class allocations
- Lesson plans and syllabus tracking
- Academic resources repository (handouts, references)

## Attendance
- Student attendance capture (daily/period-wise)
- Staff attendance capture (shifts, overtime)
- Biometric/RFID integration adapters
- Absentee alerts (SMS/email/push)
- Attendance analytics and compliance reporting

## Fees
- Fee structure and installment configuration
- Invoicing, receipts, and refund handling
- Payment gateway integrations
- Ledger reconciliation and aging reports
- Scholarship and concession management

## Salary Management
- Payroll configuration (grades, allowances, deductions)
- Payroll processing and payslip generation
- Statutory compliance (PF, ESI, tax)
- Disbursement scheduling and bank file exports
- Payroll adjustments, loans, and advances

## Inventory Management
- Item catalog, categories, and units of measure
- Stock levels, re-order rules, and alerts
- Purchase orders, goods receipt, and supplier management
- Issuance to departments and consumption tracking
- Asset tagging, depreciation, and disposal workflows

## Mark Entry
- Assessment scheme configuration (exams, internal evaluations)
- Gradebook templates and rubrics
- Bulk mark entry (web, CSV import, API)
- Moderation and approval workflows
- Result processing, GPA/CGPA calculations, transcripts

## Student Certification
- Certificate templates and numbering rules
- Request intake and approval workflows
- Auto-population from student records
- Digital signature support and document storage
- Issuance tracking and audit trail

## SMS & Notifications
- Message templates with personalization tokens
- Role-based broadcast capabilities
- Event-based triggers (attendance, fees, exams)
- Delivery status tracking and retries
- Integration layer for multiple SMS providers

## Portals & Access
- Dedicated UX flows for Students, Staff, Parents, Admin
- Role-based navigation composition
- Cross-cutting widgets (notifications, tasks, support)
- Preference management (language, theme, channels)
- Terms of use and consent tracking

## Cross-Cutting Concerns
- Audit logging and data retention policies
- Search and reporting shared services
- Document storage abstraction (local/cloud)
- Localization and multi-campus support
- Integration bus for inter-module events




