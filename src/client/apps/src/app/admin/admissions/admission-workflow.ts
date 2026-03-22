import { OnlineApplicationDto } from '@client/shared/data';

/**
 * Maps the product’s 7-stage admission workflow to fields that exist today:
 * `ApplicationStatus`, `isApplicationSubmitted`, `isPaymentCompleted`.
 * Distinct DB statuses like UNDER_REVIEW are not implemented yet — we surface
 * the closest equivalent so admins can still work in order.
 */
export const ADMISSION_WORKFLOW_STAGES = [
  {
    id: 1,
    title: 'Application intake',
    short: 'Intake',
    statuses: ['SUBMITTED (draft)', 'PAYMENT SUCCESS (application fee)'],
    adminActions: 'View / filter applications; confirm application fee paid.',
    routes: ['/admin/admissions/applications'],
  },
  {
    id: 2,
    title: 'Document verification',
    short: 'Documents',
    statuses: ['UNDER REVIEW → use Submitted + paid', 'DOCUMENT REJECTED → Rejected + remarks'],
    adminActions: 'Open application → verify uploads; set status with remarks.',
    routes: ['/admin/admissions/applications', '/admin/admissions/verification'],
  },
  {
    id: 3,
    title: 'Merit / selection',
    short: 'Merit',
    statuses: ['SELECTED → Approved', 'WAITLISTED → WaitingList', 'NOT SELECTED → Rejected'],
    adminActions: 'Merit list, entrance exam scores, manual status updates.',
    routes: ['/admin/admissions/merit-list', '/admin/admissions/exams'],
  },
  {
    id: 4,
    title: 'Admission decision',
    short: 'Decision',
    statuses: ['Publish results; notify applicants'],
    adminActions: 'Admission Approval (approve/reject), bulk communication, offers, send individual offer.',
    routes: ['/admin/admissions/approval', '/admin/admissions/send-offer'],
  },
  {
    id: 5,
    title: 'Admission fee (if applicable)',
    short: 'Admission fee',
    statuses: ['ADMISSION PAYMENT PENDING', 'ADMISSION CONFIRMED → Approved'],
    adminActions: 'Payment verification, manual payment entry.',
    routes: ['/admin/admissions/payment-verification', '/admin/admissions/payment-management'],
  },
  {
    id: 6,
    title: 'Final enrollment',
    short: 'Enrollment',
    statuses: ['ENROLLED'],
    adminActions: 'Enroll from application list; assign roll/class in student module.',
    routes: ['/admin/admissions/applications', '/admin/students/list'],
  },
  {
    id: 7,
    title: 'Dashboard & reporting',
    short: 'Reporting',
    statuses: ['KPIs'],
    adminActions: 'Dashboard analytics, exports, paid applications report.',
    routes: ['/admin/dashboard', '/admin/admissions/analytics'],
  },
] as const;

export type WorkflowStageFilter =
  | ''
  | 'intake'
  | 'documents'
  | 'merit'
  | 'decision'
  | 'fee'
  | 'enrollment';

/** Human-readable pipeline step for a row (for filters and table). */
export function getWorkflowPipelineLabel(app: OnlineApplicationDto): string {
  if (app.status === 'Enrolled') {
    return '6–7 · Enrolled';
  }
  if (!app.isApplicationSubmitted) {
    return '1 · Draft / not submitted';
  }
  if (!app.isPaymentCompleted) {
    return '1 · Submitted (application fee pending)';
  }
  if (app.status === 'Submitted') {
    return '2 · Document verification';
  }
  if (app.status === 'EntranceExam') {
    return '3 · Entrance exam / merit';
  }
  if (app.status === 'WaitingList') {
    return '3–4 · Waitlisted';
  }
  if (app.status === 'Approved') {
    return '4–6 · Selected / admission fee / enroll';
  }
  if (app.status === 'Rejected') {
    return '3–4 · Not selected (or docs rejected)';
  }
  return app.status;
}

/** Paid applicants whose status still requires an admission approve/reject decision (merit path). */
export function isPendingAdmissionDecision(app: OnlineApplicationDto): boolean {
  if (!app.isApplicationSubmitted || !app.isPaymentCompleted) {
    return false;
  }
  return (
    app.status === 'Submitted' ||
    app.status === 'EntranceExam' ||
    app.status === 'WaitingList'
  );
}

export function applicationMatchesWorkflowStage(
  app: OnlineApplicationDto,
  stage: WorkflowStageFilter
): boolean {
  if (!stage) {
    return true;
  }
  switch (stage) {
    case 'intake':
      return (
        !app.isApplicationSubmitted ||
        (app.isApplicationSubmitted && !app.isPaymentCompleted) ||
        (app.isPaymentCompleted && app.status === 'Submitted')
      );
    case 'documents':
      return (
        app.isApplicationSubmitted &&
        app.isPaymentCompleted &&
        app.status === 'Submitted'
      );
    case 'merit':
      return app.status === 'EntranceExam' || app.status === 'WaitingList';
    case 'decision':
      return (
        app.status === 'Approved' ||
        app.status === 'Rejected' ||
        app.status === 'WaitingList'
      );
    case 'fee':
      return app.status === 'Approved' && app.isPaymentCompleted;
    case 'enrollment':
      return app.status === 'Enrolled';
    default:
      return true;
  }
}
