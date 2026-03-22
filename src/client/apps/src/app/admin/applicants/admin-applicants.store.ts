import { inject, Injectable, signal } from '@angular/core';
import {
  AdminApplicantDto,
  AdmissionsAdminApiService,
  CreateApplicantPayload,
  UpdateApplicantStatusPayload,
} from '@client/shared/data';
import { ToastService } from '../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Injectable()
export class AdminApplicantsStore {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly toast = inject(ToastService);

  private readonly applicantsSignal = signal<AdminApplicantDto[]>([]);
  private readonly selectedApplicantSignal = signal<AdminApplicantDto | null>(
    null
  );
  private readonly loadingSignal = signal(false);
  private readonly statusUpdatingSignal = signal(false);
  private readonly creatingSignal = signal(false);
  private readonly errorSignal = signal<string | null>(null);

  readonly applicants = this.applicantsSignal.asReadonly();
  readonly selectedApplicant = this.selectedApplicantSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();
  readonly statusUpdating = this.statusUpdatingSignal.asReadonly();
  readonly creating = this.creatingSignal.asReadonly();
  readonly error = this.errorSignal.asReadonly();

  async loadApplicants(): Promise<void> {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);
    try {
      const applicants = await firstValueFrom(this.api.listApplicants());
      this.applicantsSignal.set(applicants);

      const selected = this.selectedApplicantSignal();
      if (selected) {
        const refreshed = applicants.find((x) => x.id === selected.id);
        if (refreshed) {
          this.selectedApplicantSignal.set(refreshed);
        }
      }
    } catch (error) {
      console.error('Failed to load applicants', error);
      this.errorSignal.set('Unable to load applicants. Please retry.');
    } finally {
      this.loadingSignal.set(false);
    }
  }

  async selectApplicant(applicantId: string): Promise<void> {
    this.errorSignal.set(null);
    try {
      const applicant = await firstValueFrom(this.api.getApplicant(applicantId));
      this.selectedApplicantSignal.set(applicant);
    } catch (error) {
      console.error('Failed to load applicant', error);
      this.errorSignal.set('Unable to load applicant details.');
      this.selectedApplicantSignal.set(null);
    }
  }

  clearSelection(): void {
    this.selectedApplicantSignal.set(null);
  }

  async createApplicant(payload: CreateApplicantPayload): Promise<string | null> {
    this.creatingSignal.set(true);
    this.errorSignal.set(null);
    try {
      const applicantId = await firstValueFrom(this.api.createApplicant(payload));
      this.toast.show('Applicant created successfully.', 'success');
      await this.loadApplicants();
      await this.selectApplicant(applicantId);
      return applicantId;
    } catch (error) {
      console.error('Failed to create applicant', error);
      this.toast.show('Failed to create applicant.', 'error');
      this.errorSignal.set('Failed to create applicant. Please try again.');
      return null;
    } finally {
      this.creatingSignal.set(false);
    }
  }

  async updateApplicantStatus(
    applicantId: string,
    payload: UpdateApplicantStatusPayload
  ): Promise<boolean> {
    this.statusUpdatingSignal.set(true);
    this.errorSignal.set(null);
    try {
      await firstValueFrom(this.api.updateApplicantStatus(applicantId, payload));
      this.toast.show('Applicant status updated.', 'success');
      await Promise.all([this.selectApplicant(applicantId), this.loadApplicants()]);
      return true;
    } catch (error) {
      console.error('Failed to update applicant status', error);
      this.toast.show('Failed to update status.', 'error');
      this.errorSignal.set('Unable to update applicant status.');
      return false;
    } finally {
      this.statusUpdatingSignal.set(false);
    }
  }
}






