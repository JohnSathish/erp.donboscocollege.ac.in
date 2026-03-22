import { Component, computed, inject, viewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ApplicantPortalStore } from './applicant-portal.store';
import { PaymentComponent } from '../payment/payment.component';
import { API_BASE_URL } from '@client/shared/util';
import { ApplicantElectiveSubjectDto } from '@client/shared/data';

const SHIFT_LABELS: Record<string, string> = {
  ShiftI: 'Shift - I (6:30 am – 9:30 am)',
  ShiftII: 'Shift - II (9:45 am – 3:30 pm)',
  ShiftIII: 'Shift - III (2:45 pm – 5:45 pm)',
  Morning: 'Shift - I (Morning)',
  Day: 'Shift - II (Day)',
  Evening: 'Shift - III (Evening)',
  'SHIFT - I (TIMING : 7.30 AM - 1.15 PM)': 'Shift - I (Legacy)',
  'SHIFT - II (TIMING : 9.45 AM - 3.30 PM)': 'Shift - II (Legacy)',
  'SHIFT - III (TIMING : 1.30 PM - 6.15 PM)': 'Shift - III (Legacy)',
};

/** Matches server/PDF format "CODE \u2014 NAME" (em dash). */
function splitElectiveCatalogLine(s: string): { code: string; name: string; full: string } | null {
  const emSep = ' \u2014 ';
  let idx = s.indexOf(emSep);
  let sepLen = emSep.length;
  if (idx < 0) {
    idx = s.indexOf(' - ');
    sepLen = 3;
  }
  if (idx < 0) {
    return null;
  }
  const code = s.slice(0, idx).trim();
  const name = s
    .slice(idx + sepLen)
    .trim()
    .replace(/\s*\(Auto Assigned\)\s*$/i, '')
    .trim();
  return { code, name: name || code, full: s };
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, PaymentComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent {
  private readonly store = inject(ApplicantPortalStore);
  private readonly apiBaseUrl = inject(API_BASE_URL);
  readonly paymentComponent = viewChild(PaymentComponent);

  readonly profile = computed(() => this.store.dashboard()?.profile ?? null);
  readonly profilePhotoUrl = computed(() => {
    const photo = this.profile()?.photoUrl?.trim();
    if (!photo) return null;
    if (/^https?:\/\//i.test(photo)) return photo;
    const root = this.apiBaseUrl.replace(/\/api\/?$/, '').replace(/\/$/, '');
    const path = photo.startsWith('/') ? photo : `/${photo}`;
    return `${root}${path}`;
  });
  readonly profileCompletionPercent = computed(() => {
    const s = this.store.summary();
    const total = s.application.steps.length;
    if (!total) return null;
    const done = s.application.steps.filter((st) => st.isComplete).length;
    return Math.round((done / total) * 100);
  });
  readonly documents = computed(
    () => this.store.dashboard()?.documents ?? []
  );
  readonly notifications = computed(
    () => this.store.dashboard()?.notifications ?? []
  );
  readonly summary = computed(() => this.store.summary());
  /** Shown only after application submission + application fee payment (API sends null otherwise). */
  readonly courseSelection = computed(
    () => this.store.dashboard()?.courseSelection ?? null
  );
  readonly incompleteSteps = computed(
    () => this.summary().application.incomplete
  );
  readonly paymentPending = computed(() => this.summary().fees.canPay);
  readonly selectionStatus = computed(() => this.summary().selection.status);
  readonly selectionNote = computed(() => this.summary().selection.note);
  readonly shiftLabel = computed(() => {
    const shift = this.profile()?.shift?.trim();
    if (!shift) {
      return 'Pending selection';
    }
    return SHIFT_LABELS[shift] ?? shift;
  });

  readonly documentsUploadedCount = computed(() =>
    this.documents().filter((d) => d.isComplete).length
  );

  statusStepDone(step: string): boolean {
    const status = this.selectionStatus();
    if (step === 'Submitted') return true;
    if (step === 'Under Review') return ['Under Review', 'Approved', 'Rejected', 'WaitingList', 'EntranceExam'].includes(status);
    if (step === 'Approved') return status === 'Approved';
    return false;
  }

  startPayment(): void {
    const amount = this.summary().fees.amountDue;
    this.paymentComponent()?.open(amount);
  }

  /**
   * Normalize API payload: nested DTO, PascalCase JSON, or legacy plain string (same as pre-refactor).
   */
  private coerceElective(raw: unknown): ApplicantElectiveSubjectDto {
    if (raw == null) {
      return { code: '', name: '—', description: null };
    }
    if (typeof raw === 'string') {
      const s = raw.trim();
      if (!s) {
        return { code: '', name: '—', description: null };
      }
      const split = splitElectiveCatalogLine(s);
      if (split) {
        return {
          code: split.code,
          name: split.name,
          description: split.full,
        };
      }
      return { code: s, name: s, description: null };
    }
    if (typeof raw === 'object') {
      const o = raw as Record<string, unknown>;
      const code = String(o['code'] ?? o['Code'] ?? '').trim();
      const name = String(o['name'] ?? o['Name'] ?? '').trim();
      const description = (o['description'] ?? o['Description'] ?? null) as string | null;
      if (!code && !name) {
        return { code: '', name: '—', description };
      }
      return {
        code,
        name: name || code || '—',
        description: description ?? null,
      };
    }
    return { code: '', name: '—', description: null };
  }

  /** Elective row: bold name + muted code; tooltip uses full catalog line from API. */
  electiveDisplay(raw: unknown): {
    name: string;
    code: string;
    showCode: boolean;
  } {
    const e = this.coerceElective(raw);
    const name = (e.name ?? '').trim() || '—';
    const code = (e.code ?? '').trim();
    const showCode = Boolean(code && code !== name);
    return { name, code, showCode };
  }

  private formatElectivePlainText(raw: unknown): string {
    const e = this.coerceElective(raw);
    const name = (e.name ?? '').trim();
    const code = (e.code ?? '').trim();
    if (!name && !code) {
      return '—';
    }
    if (!code || code === name) {
      return name || code;
    }
    return `${name} (${code})`;
  }

  /** Native tooltip: full catalog line when available. */
  electiveTooltip(raw: unknown): string | null {
    const e = this.coerceElective(raw);
    const d = e.description?.trim();
    if (d) {
      return d;
    }
    const name = (e.name ?? '').trim();
    const code = (e.code ?? '').trim();
    if (!name || name === '—') {
      return null;
    }
    if (code && code !== name) {
      return `${code} \u2014 ${name}`;
    }
    return name;
  }

  downloadCourseSummary(): void {
    const d = this.store.dashboard();
    const cs = d?.courseSelection;
    const p = d?.profile;
    if (!cs || !p) {
      return;
    }
    const lines = [
      'Don Bosco College, Tura — Application summary',
      '========================================',
      `Application reference: ${p.uniqueId}`,
      `Name: ${p.fullName}`,
      '',
      'YOUR SELECTED COURSE DETAILS',
      '-----------------------------',
      `Shift: ${cs.preferredShiftLabel}`,
      `Major: ${cs.majorSubject}`,
      `Minor: ${cs.minorSubject}`,
      '',
      'Electives',
      `MDC: ${this.formatElectivePlainText(cs.mdc)}`,
      `AEC: ${this.formatElectivePlainText(cs.aec)}`,
      `SEC: ${this.formatElectivePlainText(cs.sec)}`,
      `VAC: ${this.formatElectivePlainText(cs.vac)}`,
      '',
      cs.applicationFeePaidOnUtc
        ? `Application fee paid on: ${new Date(cs.applicationFeePaidOnUtc).toLocaleString()}`
        : '',
      cs.draftLastUpdatedOnUtc
        ? `Application data last saved: ${new Date(cs.draftLastUpdatedOnUtc).toLocaleString()}`
        : '',
    ].filter((line) => line !== '');
    const blob = new Blob([lines.join('\n')], { type: 'text/plain;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = `application-summary-${p.uniqueId}.txt`;
    anchor.click();
    URL.revokeObjectURL(url);
  }
}
