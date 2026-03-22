import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {
  OnlineApplicationDetailDto,
  AdmissionsAdminApiService,
  ApplicantStatus,
  UpdateOnlineApplicationStatusPayload,
} from '@client/shared/data';
import { ADMIN_APPLICANT_STATUSES } from '../../admin.constants';
import { firstValueFrom } from 'rxjs';
import { ToastService } from '../../../shared/toast.service';
import { getWorkflowPipelineLabel } from '../admission-workflow';

@Component({
  selector: 'app-admin-online-application-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, DatePipe, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-online-application-detail.component.html',
  styleUrls: ['./admin-online-application-detail.component.scss'],
})
export class AdminOnlineApplicationDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly fb = inject(FormBuilder);
  private readonly toast: ToastService = inject(ToastService);

  protected readonly application = signal<OnlineApplicationDetailDto | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly statusUpdating = signal(false);
  protected readonly statuses = ADMIN_APPLICANT_STATUSES;

    protected readonly statusForm = this.fb.nonNullable.group({
      status: ['', Validators.required],
      notifyApplicant: [true],
      remarks: [''],
      paymentDeadlineUtc: [''],
      entranceExam: this.fb.group({
        scheduledOnUtc: [''],
        venue: [''],
        instructions: [''],
      }),
    });

  async ngOnInit(): Promise<void> {
    const accountId = this.route.snapshot.paramMap.get('id');
    if (!accountId) {
      await this.router.navigate(['/admin/admissions']);
      return;
    }

    await this.loadApplication(accountId);
  }

  async loadApplication(accountId: string): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const app = await firstValueFrom(this.api.getOnlineApplication(accountId));
      this.application.set(app);
      this.resetStatusForm(app);
    } catch (error) {
      console.error('Failed to load application', error);
      this.error.set('Unable to load application details.');
    } finally {
      this.loading.set(false);
    }
  }

  private resetStatusForm(app: OnlineApplicationDetailDto): void {
    this.statusForm.patchValue(
      {
        status: app.status,
        notifyApplicant: true,
        remarks: app.statusRemarks ?? '',
        paymentDeadlineUtc: '',
        entranceExam: {
          scheduledOnUtc: app.entranceExamScheduledOnUtc
            ? new Date(app.entranceExamScheduledOnUtc).toISOString().slice(0, 16)
            : '',
          venue: app.entranceExamVenue ?? '',
          instructions: app.entranceExamInstructions ?? '',
        },
      },
      { emitEvent: false }
    );
  }

  async submitStatusForm(): Promise<void> {
    const currentApp = this.application();
    if (this.statusForm.invalid || !currentApp) {
      this.statusForm.markAllAsTouched();
      return;
    }

    const accountId = currentApp.id;
    const formValue = this.statusForm.getRawValue();

    // Check if status is being changed - backend doesn't allow same-to-same transitions
    if (formValue.status === currentApp.status) {
      // Check if there are any other changes (remarks or entrance exam details)
      const hasRemarksChange = formValue.remarks?.trim() !== (currentApp.statusRemarks?.trim() || '');
      
      // Check entrance exam changes
      let hasEntranceExamChange = false;
      if (formValue.status === 'EntranceExam' && formValue.entranceExam) {
        const currentScheduled = currentApp.entranceExamScheduledOnUtc 
          ? new Date(currentApp.entranceExamScheduledOnUtc).toISOString().slice(0, 16)
          : '';
        const formScheduled = formValue.entranceExam.scheduledOnUtc?.trim() || '';
        
        hasEntranceExamChange = 
          formScheduled !== currentScheduled ||
          formValue.entranceExam.venue?.trim() !== (currentApp.entranceExamVenue?.trim() || '') ||
          formValue.entranceExam.instructions?.trim() !== (currentApp.entranceExamInstructions?.trim() || '');
      }

      // If no changes at all, prevent submission
      if (!hasRemarksChange && !hasEntranceExamChange) {
        this.toast.show(
          `Cannot update status: The application is already set to "${currentApp.status}". Please select a different status or update remarks/exam details.`, 
          'error'
        );
        return;
      }
      
      // If status is same but there are other changes, we still need to change the status
      // to a different value first, then back, OR we need to allow updating remarks without status change
      // Since backend doesn't allow same-to-same, we'll show a warning
      this.toast.show(
        `Warning: Status cannot remain "${currentApp.status}". Please select a different status to save your changes.`, 
        'error'
      );
      return;
    }

    // Format datetime-local value to ISO string if present
    let scheduledOnUtc: string | null = null;
    if (formValue.entranceExam?.scheduledOnUtc?.trim()) {
      // Convert datetime-local format (YYYY-MM-DDTHH:mm) to ISO 8601 format
      const localDateTime = formValue.entranceExam.scheduledOnUtc.trim();
      scheduledOnUtc = new Date(localDateTime).toISOString();
    }

    // Format payment deadline datetime-local to ISO string if present
    let paymentDeadlineUtc: string | null = null;
    if (formValue.paymentDeadlineUtc?.trim()) {
      const localDateTime = formValue.paymentDeadlineUtc.trim();
      paymentDeadlineUtc = new Date(localDateTime).toISOString();
    }

    const payload: UpdateOnlineApplicationStatusPayload = {
      status: formValue.status as ApplicantStatus,
      notifyApplicant: formValue.notifyApplicant ?? false,
      remarks: formValue.remarks?.trim() || null,
      paymentDeadlineUtc: paymentDeadlineUtc,
      entranceExam:
        formValue.status === 'EntranceExam'
          ? {
              scheduledOnUtc: scheduledOnUtc,
              venue: formValue.entranceExam?.venue?.trim() || null,
              instructions:
                formValue.entranceExam?.instructions?.trim() || null,
            }
          : null,
    };

    console.log('Updating application status:', { accountId, payload });

    this.statusUpdating.set(true);
    try {
      const updated = await firstValueFrom(
        this.api.updateOnlineApplicationStatus(accountId, payload)
      );
      this.application.set(updated);
      this.resetStatusForm(updated);
      this.toast.show('Application status updated successfully.', 'success');
    } catch (error: any) {
      console.error('Failed to update status', error);
      
      // Extract error message from response
      let errorMessage = 'Failed to update application status.';
      if (error?.error?.message) {
        errorMessage = error.error.message;
      } else if (error?.error) {
        errorMessage = typeof error.error === 'string' ? error.error : errorMessage;
      } else if (error?.message) {
        errorMessage = error.message;
      }
      
      // Check for specific HTTP errors
      if (error?.status === 400) {
        // Check if it's a status transition error
        if (errorMessage?.toLowerCase().includes('cannot transition') || 
            errorMessage?.toLowerCase().includes('transition')) {
          errorMessage = errorMessage || 'Cannot change status to the same value. Please select a different status.';
        } else {
          errorMessage = errorMessage || 'Invalid request. Please check the form data.';
        }
      } else if (error?.status === 404) {
        errorMessage = 'Application not found.';
      } else if (error?.status === 401 || error?.status === 403) {
        errorMessage = 'You do not have permission to update this application.';
      } else if (error?.status === 0) {
        errorMessage = 'Unable to connect to server. Please ensure the backend API is running.';
      }
      
      this.toast.show(errorMessage, 'error');
    } finally {
      this.statusUpdating.set(false);
    }
  }

  async goBack(): Promise<void> {
    await this.router.navigate(['/admin/admissions/applications']);
  }

  getStatusIcon(status: string): string {
    const iconMap: { [key: string]: string } = {
      'Submitted': 'solar:document-text-bold',
      'Approved': 'solar:check-circle-bold',
      'Rejected': 'solar:close-circle-bold',
      'WaitingList': 'solar:clock-circle-bold',
      'EntranceExam': 'solar:document-text-bold',
      'Enrolled': 'solar:diploma-bold',
    };
    return iconMap[status] || 'solar:circle-bold';
  }

  hasOtherChanges(): boolean {
    const currentApp = this.application();
    if (!currentApp) return false;

    const formValue = this.statusForm.getRawValue();
    
    // Check if remarks have changed
    const hasRemarksChange = formValue.remarks?.trim() !== (currentApp.statusRemarks?.trim() || '');
    
    // Check if entrance exam details have changed
    let hasEntranceExamChange = false;
    if (formValue.status === 'EntranceExam' && formValue.entranceExam) {
      const currentScheduled = currentApp.entranceExamScheduledOnUtc 
        ? new Date(currentApp.entranceExamScheduledOnUtc).toISOString().slice(0, 16)
        : '';
      const formScheduled = formValue.entranceExam.scheduledOnUtc?.trim() || '';
      
      hasEntranceExamChange = 
        formScheduled !== currentScheduled ||
        formValue.entranceExam.venue?.trim() !== (currentApp.entranceExamVenue?.trim() || '') ||
        formValue.entranceExam.instructions?.trim() !== (currentApp.entranceExamInstructions?.trim() || '');
    }
    
    return hasRemarksChange || hasEntranceExamChange;
  }

  workflowPipelineLabel(): string {
    const app = this.application();
    if (!app) {
      return '';
    }
    return getWorkflowPipelineLabel(app);
  }
}

