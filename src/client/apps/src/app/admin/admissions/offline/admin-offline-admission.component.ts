import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  AdmissionsAdminApiService,
  AdminDashboardDto,
  OfflineFormIssuancePreviewDto,
} from '@client/shared/data';
import { finalize } from 'rxjs/operators';
import { ToastService } from '../../../shared/toast.service';

@Component({
  selector: 'app-admin-offline-admission',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-offline-admission.component.html',
  styleUrls: ['./admin-offline-admission.component.scss'],
})
export class AdminOfflineAdmissionComponent implements OnInit {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly toast = inject(ToastService);

  readonly loading = signal(false);
  readonly dashboard = signal<AdminDashboardDto | null>(null);

  issue = {
    formNumber: '',
    studentName: '',
    mobileNumber: '',
    applicationFeeAmount: 0,
  };

  receiveFormNumber = '';
  receiveMajorSubject = '';
  readonly receivePreview = signal<OfflineFormIssuancePreviewDto | null | undefined>(undefined);

  assignApplicationId = '';
  assignRound: 'First' | 'Second' | 'Third' = 'First';
  publishRound: 'First' | 'Second' | 'Third' = 'First';
  publishSendSms = true;
  reprintFormNumber = '';

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading.set(true);
    this.api
      .getAdminDashboard()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (d) => this.dashboard.set(d),
        error: () => this.toast.error('Could not load admission dashboard.'),
      });
  }

  issueForm(): void {
    const v = this.issue;
    if (!v.formNumber.trim() || v.formNumber.trim().length !== 6) {
      this.toast.error('Enter a 6-digit form number.');
      return;
    }
    this.loading.set(true);
    this.api
      .issueOfflineAdmissionForm({
        formNumber: v.formNumber.trim(),
        studentName: v.studentName.trim(),
        mobileNumber: v.mobileNumber.trim(),
        applicationFeeAmount: v.applicationFeeAmount,
      })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (blob) => {
          const url = URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `offline-receipt-${v.formNumber.trim()}.pdf`;
          a.click();
          URL.revokeObjectURL(url);
          this.toast.success('Form issued. Receipt PDF downloaded (no portal account yet).');
          this.loadDashboard();
        },
        error: (err) => {
          this.toast.error(this.errMsg(err) ?? 'Issue failed.');
        },
      });
  }

  lookupReceivePreview(): void {
    const n = this.receiveFormNumber.trim();
    this.receivePreview.set(undefined);
    if (n.length !== 6) {
      return;
    }
    this.loading.set(true);
    this.api
      .getOfflineFormIssuancePreview(n)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (p) => this.receivePreview.set(p),
        error: () => {
          this.receivePreview.set(null);
          this.toast.error('Could not look up form.');
        },
      });
  }

  receiveForm(): void {
    const n = this.receiveFormNumber.trim();
    const major = this.receiveMajorSubject.trim();
    if (n.length !== 6) {
      this.toast.error('Enter a 6-digit form number.');
      return;
    }
    if (!major) {
      this.toast.error('Enter the student’s final major / course choice.');
      return;
    }
    const p = this.receivePreview();
    if (p?.applicantAccountCreated) {
      this.toast.error('An applicant account already exists for this form.');
      return;
    }
    this.loading.set(true);
    this.api
      .receiveOfflineAdmissionForm({ formNumber: n, majorSubject: major })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (r) => {
          this.toast.success(
            `Account created: ${r.studentName} — ${r.majorSubject}. Form marked received.`
          );
          this.receiveFormNumber = '';
          this.receiveMajorSubject = '';
          this.receivePreview.set(undefined);
          this.loadDashboard();
        },
        error: (err) => this.toast.error(this.errMsg(err) ?? 'Receive failed.'),
      });
  }

  reprintReceipt(): void {
    const n = this.reprintFormNumber.trim();
    if (n.length !== 6) {
      this.toast.error('Enter a 6-digit form number.');
      return;
    }
    this.loading.set(true);
    this.api
      .getOfflineFormReceiptPdf(n)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (blob) => {
          const url = URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `offline-receipt-${n}.pdf`;
          a.click();
          URL.revokeObjectURL(url);
        },
        error: (err) => this.toast.error(this.errMsg(err) ?? 'Could not load receipt.'),
      });
  }

  assignRoundSave(): void {
    const id = this.assignApplicationId.trim();
    if (!id) {
      this.toast.error('Enter application (account) ID GUID.');
      return;
    }
    this.loading.set(true);
    this.api
      .assignSelectionListRound(id, this.assignRound)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: () => this.toast.success('Selection list round saved.'),
        error: (err) => this.toast.error(this.errMsg(err) ?? 'Save failed.'),
      });
  }

  publishList(): void {
    this.loading.set(true);
    this.api
      .publishSelectionList({
        round: this.publishRound,
        sendSmsNotifications: this.publishSendSms,
      })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (r) =>
          this.toast.success(
            `Published: ${r.publishedCount}. SMS sent: ${r.smsSent}, failed: ${r.smsFailed}.`
          ),
        error: (err) => this.toast.error(this.errMsg(err) ?? 'Publish failed.'),
      });
  }

  private errMsg(err: unknown): string | null {
    const e = err as { error?: unknown; message?: string };
    if (typeof e?.error === 'string' && e.error.length) {
      return e.error;
    }
    if (e?.error && typeof e.error === 'object' && 'message' in (e.error as object)) {
      return String((e.error as { message?: unknown }).message);
    }
    if (typeof e?.message === 'string') {
      return e.message;
    }
    return null;
  }
}
