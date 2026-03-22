import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
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
import {
  ExaminationsApiService,
  ResultSummaryDto,
  CourseResultDto,
} from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { AuthService } from '../../../auth/auth.service';
import { LoggingService } from '../../../shared/logging.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-result-processing',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-result-processing.component.html',
  styleUrls: ['./admin-result-processing.component.scss'],
})
export class AdminResultProcessingComponent implements OnInit {
  private readonly api = inject(ExaminationsApiService);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);
  private readonly authService = inject(AuthService);
  private readonly logger = inject(LoggingService);

  protected readonly result = signal<ResultSummaryDto | null>(null);
  protected readonly loading = signal(false);
  protected readonly processing = signal(false);
  protected readonly error = signal<string | null>(null);

  protected readonly searchForm = this.fb.group({
    studentId: ['', [Validators.required]],
    academicTermId: ['', [Validators.required]],
  });

  protected readonly gpa = computed(() => {
    const result = this.result();
    return result?.gpa ?? null;
  });

  protected readonly percentage = computed(() => {
    const result = this.result();
    return result?.percentage ?? 0;
  });

  protected readonly totalMarks = computed(() => {
    const result = this.result();
    return result?.totalMarks ?? 0;
  });

  protected readonly maxMarks = computed(() => {
    const result = this.result();
    return result?.maxMarks ?? 0;
  });

  ngOnInit(): void {
    // Component initialized
  }

  async processResults(): Promise<void> {
    if (this.searchForm.invalid) {
      this.searchForm.markAllAsTouched();
      this.toast.show('Please fill in all required fields', 'error');
      return;
    }

    const { studentId, academicTermId } = this.searchForm.getRawValue();
    if (!studentId || !academicTermId) {
      this.toast.show('Please select both student and academic term', 'error');
      return;
    }

    this.processing.set(true);
    this.error.set(null);
    try {
      const result = await firstValueFrom(
        this.api.processResults({
          studentId,
          academicTermId,
          processedBy: this.getCurrentUser(),
        })
      );

      this.toast.show(
        `Results processed successfully. Result Summary ID: ${result.resultSummaryId}`,
        'success'
      );

      // Load the processed results
      await this.loadResults();
    } catch (error: any) {
      this.logger.error('Failed to process results', error);
      this.error.set(error.error?.message || 'Failed to process results. Please try again.');
      this.toast.show(
        error.error?.message || 'Failed to process results. Please try again.',
        'error'
      );
    } finally {
      this.processing.set(false);
    }
  }

  async loadResults(): Promise<void> {
    const { studentId, academicTermId } = this.searchForm.getRawValue();
    if (!studentId || !academicTermId) {
      return;
    }

    this.loading.set(true);
    this.error.set(null);
    try {
      const result = await firstValueFrom(
        this.api.getResults(studentId, academicTermId)
      );
      this.result.set(result);
    } catch (error: any) {
      this.logger.error('Failed to load results', error);
      if (error.status === 404) {
        this.error.set('No results found for this student and term. Process results first.');
        this.result.set(null);
      } else {
        this.error.set('Failed to load results. Please try again.');
        this.toast.show('Failed to load results', 'error');
      }
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    await this.loadResults();
  }

  getGradeClass(grade: string | null | undefined): string {
    if (!grade) return 'badge--neutral';
    
    const gradeUpper = grade.toUpperCase();
    if (['A+', 'A', 'A-'].includes(gradeUpper)) {
      return 'badge--success';
    } else if (['B+', 'B', 'B-'].includes(gradeUpper)) {
      return 'badge--info';
    } else if (['C+', 'C', 'C-'].includes(gradeUpper)) {
      return 'badge--warning';
    } else if (['D', 'F'].includes(gradeUpper)) {
      return 'badge--danger';
    }
    return 'badge--neutral';
  }

  getResultStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'passed':
      case 'pass':
        return 'badge--success';
      case 'failed':
      case 'fail':
        return 'badge--danger';
      case 'pending':
        return 'badge--warning';
      default:
        return 'badge--neutral';
    }
  }

  private getCurrentUser(): string {
    const profile = this.authService.profile;
    return profile?.fullName || profile?.email || profile?.uniqueId || 'System';
  }
}
