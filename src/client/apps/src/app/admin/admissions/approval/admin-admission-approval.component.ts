import { Component, OnInit, inject, signal, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { OnlineApplicationDto, ApplicantStatus } from '@client/shared/data';
import { firstValueFrom } from 'rxjs';
import { AdmissionsAdminApiService } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { isPendingAdmissionDecision } from '../admission-workflow';

@Component({
  selector: 'app-admin-admission-approval',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './admin-admission-approval.component.html',
  styleUrls: ['./admin-admission-approval.component.scss'],
})
export class AdminAdmissionApprovalComponent implements OnInit {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly applications = signal<OnlineApplicationDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly directAdmissionCutoff = signal(75);
  protected readonly actionAccountId = signal<string | null>(null);

  protected readonly searchControl = this.fb.control<string>('');
  protected readonly shiftControl = this.fb.control<string>('');
  protected readonly statusFilterControl = this.fb.control<
    'all' | 'Submitted' | 'EntranceExam' | 'WaitingList'
  >('all');
  protected readonly meritEligibleOnlyControl = this.fb.control<boolean>(false);

  async ngOnInit(): Promise<void> {
    try {
      const s = await firstValueFrom(this.api.getAdmissionWorkflowSettings());
      this.directAdmissionCutoff.set(s.directAdmissionCutoffPercentage);
    } catch {
      /* default */
    }
    await this.loadQueue();
  }

  displayedRows(): OnlineApplicationDto[] {
    let list = this.applications().filter((a) => isPendingAdmissionDecision(a));
    const q = this.searchControl.value?.trim().toLowerCase() ?? '';
    if (q) {
      list = list.filter(
        (a) =>
          a.uniqueId.toLowerCase().includes(q) ||
          a.fullName.toLowerCase().includes(q) ||
          a.email.toLowerCase().includes(q)
      );
    }
    const st = this.statusFilterControl.value;
    if (st && st !== 'all') {
      list = list.filter((a) => a.status === st);
    }
    return list.sort(
      (a, b) =>
        new Date(b.statusUpdatedOnUtc).getTime() - new Date(a.statusUpdatedOnUtc).getTime()
    );
  }

  async loadQueue(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    const shift = this.shiftControl.value?.trim() || undefined;
    const minPct =
      this.meritEligibleOnlyControl.value === true
        ? this.directAdmissionCutoff()
        : undefined;
    const base = {
      page: 1,
      pageSize: 200,
      isApplicationSubmitted: true,
      isPaymentCompleted: true,
      shift,
      sortBy: 'classXiiPercentage',
      sortDescending: true,
      minClassXiiPercentage: minPct,
      admissionPath: 'normal' as const,
    };
    try {
      const [r1, r2, r3] = await Promise.all([
        firstValueFrom(this.api.listOnlineApplications({ ...base, status: 'Submitted' })),
        firstValueFrom(this.api.listOnlineApplications({ ...base, status: 'EntranceExam' })),
        firstValueFrom(this.api.listOnlineApplications({ ...base, status: 'WaitingList' })),
      ]);
      const map = new Map<string, OnlineApplicationDto>();
      for (const r of [r1, r2, r3]) {
        for (const app of r.applications) {
          map.set(app.id, app);
        }
      }
      this.applications.set([...map.values()]);
    } catch (e) {
      console.error(e);
      this.error.set('Unable to load the approval queue. Please retry.');
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    await this.loadQueue();
  }

  async onFilterChange(): Promise<void> {
    await this.loadQueue();
  }

  trackByApplicationId(_index: number, item: OnlineApplicationDto): string {
    return item.id;
  }

  formatPct(app: OnlineApplicationDto): string {
    const p = app.classXIIPercentage;
    if (p === null || p === undefined) {
      return '—';
    }
    return `${Number(p).toFixed(2)}%`;
  }

  rowMeetsCutoff(app: OnlineApplicationDto): boolean {
    const p = app.classXIIPercentage;
    if (p === null || p === undefined) {
      return false;
    }
    return p >= this.directAdmissionCutoff();
  }

  async viewApplication(id: string): Promise<void> {
    await this.router.navigate(['/admin/admissions/applications', id]);
  }

  async approveApp(app: OnlineApplicationDto): Promise<void> {
    if (!window.confirm(`Approve admission for ${app.uniqueId} — ${app.fullName}?`)) {
      return;
    }
    this.actionAccountId.set(app.id);
    try {
      await firstValueFrom(this.api.approveOnlineApplication(app.id, { notifyApplicant: true }));
      this.toast.success('Application approved.');
      await this.loadQueue();
    } catch (e: unknown) {
      this.toast.error(this.extractError(e) ?? 'Approve failed');
    } finally {
      this.actionAccountId.set(null);
    }
  }

  async rejectApp(app: OnlineApplicationDto): Promise<void> {
    const remarks = window.prompt(
      `Reject admission for ${app.uniqueId}? Optional remarks (shown to applicant if configured):`,
      ''
    );
    if (remarks === null) {
      return;
    }
    this.actionAccountId.set(app.id);
    try {
      await firstValueFrom(
        this.api.rejectOnlineApplication(app.id, {
          notifyApplicant: true,
          remarks: remarks.trim() || undefined,
        })
      );
      this.toast.success('Application rejected.');
      await this.loadQueue();
    } catch (e: unknown) {
      this.toast.error(this.extractError(e) ?? 'Reject failed');
    } finally {
      this.actionAccountId.set(null);
    }
  }

  private extractError(e: unknown): string | undefined {
    const err = e as { error?: unknown; message?: string };
    if (typeof err.error === 'string') {
      return err.error;
    }
    if (err.error && typeof err.error === 'object' && 'message' in err.error) {
      return String((err.error as { message?: string }).message);
    }
    return err.message;
  }

  statusBadgeClass(status: ApplicantStatus): string {
    switch (status) {
      case 'Approved':
      case 'Enrolled':
        return 'status-approved';
      case 'Rejected':
        return 'status-rejected';
      case 'WaitingList':
        return 'status-waiting';
      case 'EntranceExam':
        return 'status-exam';
      default:
        return 'status-submitted';
    }
  }
}
