import {
  ChangeDetectionStrategy,
  Component,
  EffectRef,
  OnDestroy,
  OnInit,
  effect,
  inject,
} from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ADMIN_APPLICANT_STATUSES } from '../admin.constants';
import {
  ApplicantStatus,
  UpdateApplicantStatusPayload,
} from '@client/shared/data';
import { AdminApplicantsStore } from './admin-applicants.store';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-admin-applicant-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, DatePipe],
  providers: [AdminApplicantsStore],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-applicant-detail.component.html',
  styleUrls: ['./admin-applicant-detail.component.scss'],
})
export class AdminApplicantDetailComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly store = inject(AdminApplicantsStore);
  private readonly fb = inject(FormBuilder);

  protected readonly applicant = this.store.selectedApplicant;
  protected readonly loading = this.store.loading;
  protected readonly error = this.store.error;
  protected readonly statusUpdating = this.store.statusUpdating;
  protected readonly statuses = ADMIN_APPLICANT_STATUSES;

  private readonly routeSub = new Subscription();
  private effectRef: EffectRef | null = null;

  protected readonly statusForm = this.fb.nonNullable.group({
    status: ['', Validators.required],
    notifyApplicant: [true],
    remarks: [''],
    entranceExam: this.fb.group({
      scheduledOnUtc: [''],
      venue: [''],
      instructions: [''],
    }),
  });

  async ngOnInit(): Promise<void> {
    this.routeSub.add(
      this.route.paramMap.subscribe(async (params) => {
        const applicantId = params.get('id');
        if (!applicantId) {
          await this.router.navigate(['/admin/admissions']);
          return;
        }
        await this.store.selectApplicant(applicantId);
        await this.store.loadApplicants();
        const applicant = this.store.selectedApplicant();
        if (!applicant) {
          await this.router.navigate(['/admin/admissions']);
          return;
        }
        this.resetStatusForm(applicant);
      })
    );

    this.effectRef = effect(() => {
      const applicant = this.store.selectedApplicant();
      if (applicant) {
        this.resetStatusForm(applicant);
      }
    });
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
    this.effectRef?.destroy();
  }

  private resetStatusForm(applicant: {
    status: ApplicantStatus;
    statusRemarks?: string | null;
    entranceExamScheduledOnUtc?: string | null;
    entranceExamVenue?: string | null;
    entranceExamInstructions?: string | null;
  }): void {
    this.statusForm.patchValue(
      {
        status: applicant.status,
        notifyApplicant: true,
        remarks: applicant.statusRemarks ?? '',
        entranceExam: {
          scheduledOnUtc: applicant.entranceExamScheduledOnUtc ?? '',
          venue: applicant.entranceExamVenue ?? '',
          instructions: applicant.entranceExamInstructions ?? '',
        },
      },
      { emitEvent: false }
    );
  }

  async submitStatusForm(): Promise<void> {
    const currentApplicant = this.store.selectedApplicant();
    if (this.statusForm.invalid || !currentApplicant) {
      this.statusForm.markAllAsTouched();
      return;
    }

    const applicantId = currentApplicant.id;
    const formValue = this.statusForm.getRawValue();

    const payload: UpdateApplicantStatusPayload = {
      status: formValue.status as ApplicantStatus,
      notifyApplicant: formValue.notifyApplicant ?? false,
      remarks: formValue.remarks?.trim() || null,
      entranceExam:
        formValue.status === 'EntranceExam'
          ? {
              scheduledOnUtc:
                formValue.entranceExam?.scheduledOnUtc?.trim() || null,
              venue: formValue.entranceExam?.venue?.trim() || null,
              instructions:
                formValue.entranceExam?.instructions?.trim() || null,
            }
          : null,
    };

    await this.store.updateApplicantStatus(applicantId, payload);
  }

  async goBack(): Promise<void> {
    await this.router.navigate(['/admin/admissions']);
  }

  trackByStatus(index: number, status: ApplicantStatus): ApplicantStatus {
    return status;
  }
}

