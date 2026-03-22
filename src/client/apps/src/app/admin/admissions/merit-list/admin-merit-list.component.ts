import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  AdmissionsAdminApiService,
  GenerateMeritListResponse,
  MeritListResponse,
} from '@client/shared/data';
import { finalize } from 'rxjs/operators';
import { ToastService } from '../../../shared/toast.service';

@Component({
  selector: 'app-admin-merit-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-merit-list.component.html',
  styleUrls: ['./admin-merit-list.component.scss'],
})
export class AdminMeritListComponent implements OnInit {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly toast = inject(ToastService);

  readonly loading = signal(false);
  readonly generating = signal(false);
  readonly error = signal<string | null>(null);
  readonly meritList = signal<MeritListResponse | null>(null);

  readonly filters = signal({
    shift: '',
    majorSubject: '',
  });

  readonly page = signal(1);
  readonly pageSize = 50;

  /** Minimum Class XII % for merit list & direct admission (stored in database). */
  readonly meritCutoffDraft = signal<number>(75);
  readonly savingSettings = signal(false);
  readonly settingsLoading = signal(true);

  ngOnInit(): void {
    this.loadWorkflowSettings();
    this.loadMeritList();
  }

  private loadWorkflowSettings(): void {
    this.settingsLoading.set(true);
    this.api.getAdmissionWorkflowSettings().subscribe({
      next: (s) => {
        this.meritCutoffDraft.set(Number(s.directAdmissionCutoffPercentage));
        this.settingsLoading.set(false);
      },
      error: () => {
        this.settingsLoading.set(false);
        this.toast.error('Could not load merit percentage settings.');
      },
    });
  }

  saveMeritCutoff(): void {
    const v = Number(this.meritCutoffDraft());
    if (Number.isNaN(v) || v < 0 || v > 100) {
      this.toast.error('Enter a percentage between 0 and 100.');
      return;
    }
    this.savingSettings.set(true);
    this.api
      .updateAdmissionWorkflowSettings({ directAdmissionCutoffPercentage: v })
      .pipe(finalize(() => this.savingSettings.set(false)))
      .subscribe({
        next: (s) => {
          this.meritCutoffDraft.set(Number(s.directAdmissionCutoffPercentage));
          this.toast.success('Merit percentage saved. Regenerate the merit list to apply to new rankings.');
          this.loadMeritList();
        },
        error: (err) => {
          const msg =
            err?.error?.message ??
            err?.error ??
            (typeof err?.message === 'string' ? err.message : 'Save failed.');
          this.toast.error(String(msg));
        },
      });
  }

  loadMeritList(): void {
    this.loading.set(true);
    this.error.set(null);

    this.api
      .getMeritList(
        this.filters().shift || undefined,
        this.filters().majorSubject || undefined,
        this.page(),
        this.pageSize
      )
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (response) => {
          this.meritList.set(response);
        },
        error: (error) => {
          this.error.set(
            error?.error?.message || 'Failed to load merit list.'
          );
        },
      });
  }

  generateMeritList(): void {
    this.generating.set(true);
    this.error.set(null);

    this.api
      .generateMeritList(
        this.filters().shift || undefined,
        this.filters().majorSubject || undefined
      )
      .pipe(finalize(() => this.generating.set(false)))
      .subscribe({
        next: (response: GenerateMeritListResponse) => {
          this.loadMeritList();
          alert(
            `Merit list generated successfully! Processed ${response.totalApplicantsProcessed} applicants, created ${response.meritScoresCreated} merit scores.`
          );
        },
        error: (error) => {
          this.error.set(
            error?.error?.message || 'Failed to generate merit list.'
          );
        },
      });
  }

  applyFilters(): void {
    this.page.set(1);
    this.loadMeritList();
  }

  clearFilters(): void {
    this.filters.set({ shift: '', majorSubject: '' });
    this.page.set(1);
    this.loadMeritList();
  }

  onPageChange(newPage: number): void {
    this.page.set(newPage);
    this.loadMeritList();
  }

  createOffer(accountId: string): void {
    const expiryDate = new Date();
    expiryDate.setDate(expiryDate.getDate() + 30); // 30 days from now

    if (
      !confirm(
        'Create admission offer for this applicant? The offer will expire in 30 days.'
      )
    ) {
      return;
    }

    this.api
      .createAdmissionOffer(accountId, expiryDate.toISOString())
      .subscribe({
        next: (response) => {
          alert(
            `Admission offer created successfully for ${response.fullName}!`
          );
        },
        error: (error) => {
          alert(
            error?.error?.message || 'Failed to create admission offer.'
          );
        },
      });
  }

  readonly Math = Math;
}
