import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
  inject,
  signal,
} from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AdminApplicantsStore } from './admin-applicants.store';
import { ADMIN_APPLICANT_STATUSES } from '../admin.constants';
import { CreateApplicantPayload } from '@client/shared/data';

@Component({
  selector: 'app-admin-applicants-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, DatePipe],
  providers: [AdminApplicantsStore],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-applicants-list.component.html',
  styleUrls: ['./admin-applicants-list.component.scss'],
})
export class AdminApplicantsListComponent implements OnInit {
  private readonly store = inject(AdminApplicantsStore);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  protected readonly applicants = this.store.applicants;
  protected readonly loading = this.store.loading;
  protected readonly error = this.store.error;
  protected readonly creating = this.store.creating;

  protected readonly showCreateForm = signal(false);
  protected readonly statusFilters = ADMIN_APPLICANT_STATUSES;

  protected readonly filterControl = this.fb.control<string | null>(null);

  protected readonly createForm = this.fb.nonNullable.group({
    applicationNumber: ['', [Validators.required, Validators.maxLength(32)]],
    firstName: ['', [Validators.required, Validators.maxLength(128)]],
    lastName: ['', [Validators.required, Validators.maxLength(128)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    mobileNumber: [
      '',
      [
        Validators.required,
        Validators.pattern(/^[0-9+\-\s]{8,15}$/),
        Validators.maxLength(32),
      ],
    ],
    dateOfBirth: ['', Validators.required],
    programCode: ['', [Validators.required, Validators.maxLength(32)]],
  });

  protected readonly filteredApplicants = computed(() => {
    const statusFilter = this.filterControl.value;
    const applicants = this.applicants();
    if (!statusFilter) {
      return applicants;
    }
    return applicants.filter((applicant) => applicant.status === statusFilter);
  });

  async ngOnInit(): Promise<void> {
    await this.store.loadApplicants();
  }

  toggleCreateForm(): void {
    this.showCreateForm.update((value) => !value);
  }

  async submitCreateForm(): Promise<void> {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    const formValue = this.createForm.getRawValue();
    const payload: CreateApplicantPayload = {
      applicationNumber: formValue.applicationNumber.trim(),
      firstName: formValue.firstName.trim(),
      lastName: formValue.lastName.trim(),
      email: formValue.email.trim(),
      mobileNumber: formValue.mobileNumber.trim(),
      dateOfBirth: formValue.dateOfBirth,
      programCode: formValue.programCode.trim(),
    };

    const result = await this.store.createApplicant(payload);
    if (result) {
      this.createForm.reset();
      this.toggleCreateForm();
      await this.viewApplicant(result);
    }
  }

  async refresh(): Promise<void> {
    await this.store.loadApplicants();
  }

  async viewApplicant(applicantId: string): Promise<void> {
    await this.router.navigate(['/admin/admissions', applicantId]);
  }

  trackByApplicantId(index: number, item: { id: string }): string {
    return item.id;
  }
}

