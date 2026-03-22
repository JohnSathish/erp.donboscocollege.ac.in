import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AdmissionsAdminApiService } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-bulk-communication',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-bulk-communication.component.html',
  styleUrls: ['./admin-bulk-communication.component.scss'],
})
export class AdminBulkCommunicationComponent {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly communicationForm: FormGroup;
  protected readonly sending = signal(false);
  protected readonly result = signal<{
    totalRecipients: number;
    emailsSent: number;
    smsSent: number;
    emailsFailed: number;
    smsFailed: number;
    errors: Array<{
      accountId: string;
      applicationNumber: string;
      errorMessage: string;
      channel: string;
    }>;
  } | null>(null);

  protected readonly statusOptions = [
    { value: 'Submitted', label: 'Submitted' },
    { value: 'Approved', label: 'Approved' },
    { value: 'Rejected', label: 'Rejected' },
    { value: 'WaitingList', label: 'Waiting List' },
    { value: 'EntranceExam', label: 'Entrance Exam' },
    { value: 'Enrolled', label: 'Enrolled' },
  ];

  protected readonly channelOptions = [
    { value: 'Email', label: 'Email Only' },
    { value: 'Sms', label: 'SMS Only' },
    { value: 'Both', label: 'Email & SMS' },
  ];

  constructor() {
    this.communicationForm = this.fb.group({
      subject: ['', [Validators.required, Validators.maxLength(200)]],
      message: ['', [Validators.required, Validators.maxLength(2000)]],
      channel: ['Email', Validators.required],
      filter: this.fb.group({
        statuses: [[]],
        isApplicationSubmitted: [null],
        isPaymentCompleted: [null],
        searchTerm: [''],
      }),
    });
  }

  async sendCommunication(): Promise<void> {
    if (this.communicationForm.invalid) {
      this.communicationForm.markAllAsTouched();
      this.toast.error('Please fill in all required fields correctly.');
      return;
    }

    const formValue = this.communicationForm.value;
    const filter = formValue.filter;

    // Build filter object
    const filterPayload: any = {};
    if (filter.statuses && filter.statuses.length > 0) {
      filterPayload.statuses = filter.statuses;
    }
    if (filter.isApplicationSubmitted !== null) {
      filterPayload.isApplicationSubmitted = filter.isApplicationSubmitted;
    }
    if (filter.isPaymentCompleted !== null) {
      filterPayload.isPaymentCompleted = filter.isPaymentCompleted;
    }
    if (filter.searchTerm?.trim()) {
      filterPayload.searchTerm = filter.searchTerm.trim();
    }

    const payload = {
      subject: formValue.subject,
      message: formValue.message,
      channel: formValue.channel,
      filter: Object.keys(filterPayload).length > 0 ? filterPayload : null,
    };

    this.sending.set(true);
    this.result.set(null);

    try {
      const response = await firstValueFrom(
        this.api.sendBulkCommunication(payload)
      );
      this.result.set(response);
      this.toast.success(
        `Communication sent! ${response.emailsSent} emails, ${response.smsSent} SMS messages sent successfully.`
      );
    } catch (error: any) {
      console.error('Failed to send bulk communication', error);
      const errorMessage =
        error?.error?.message || error?.message || 'Failed to send communication';
      this.toast.error(errorMessage);
    } finally {
      this.sending.set(false);
    }
  }

  resetForm(): void {
    this.communicationForm.reset({
      subject: '',
      message: '',
      channel: 'Email',
      filter: {
        statuses: [],
        isApplicationSubmitted: null,
        isPaymentCompleted: null,
        searchTerm: '',
      },
    });
    this.result.set(null);
  }

  toggleStatus(status: string, event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const statusesControl = this.communicationForm.get('filter.statuses');
    const currentStatuses = statusesControl?.value || [];

    if (checkbox.checked) {
      if (!currentStatuses.includes(status)) {
        statusesControl?.setValue([...currentStatuses, status]);
      }
    } else {
      statusesControl?.setValue(currentStatuses.filter((s: string) => s !== status));
    }
  }

  isStatusSelected(status: string): boolean {
    const statuses = this.communicationForm.get('filter.statuses')?.value || [];
    return statuses.includes(status);
  }
}

