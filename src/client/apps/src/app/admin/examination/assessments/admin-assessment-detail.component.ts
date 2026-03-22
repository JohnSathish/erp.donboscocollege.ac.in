import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { ExaminationsApiService, AssessmentDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { AuthService } from '../../../auth/auth.service';
import { LoggingService } from '../../../shared/logging.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-assessment-detail',
  standalone: true,
  imports: [CommonModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-assessment-detail.component.html',
  styleUrls: ['./admin-assessment-detail.component.scss'],
})
export class AdminAssessmentDetailComponent implements OnInit {
  private readonly api = inject(ExaminationsApiService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly toast = inject(ToastService);
  private readonly authService = inject(AuthService);
  private readonly logger = inject(LoggingService);

  protected readonly assessment = signal<AssessmentDto | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);

  async ngOnInit(): Promise<void> {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      await this.loadAssessment(id);
    } else {
      this.error.set('Assessment ID is required');
    }
  }

  async loadAssessment(id: string): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const assessment = await firstValueFrom(this.api.getAssessment(id));
      this.assessment.set(assessment);
    } catch (error: any) {
      this.logger.error('Failed to load assessment', error);
      if (error.status === 404) {
        this.error.set('Assessment not found');
      } else {
        this.error.set('Failed to load assessment. Please try again.');
      }
      this.toast.show('Failed to load assessment', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async publishAssessment(): Promise<void> {
    const assessment = this.assessment();
    if (!assessment) return;

    try {
      await firstValueFrom(
        this.api.publishAssessment(assessment.id, {
          publishedBy: this.getCurrentUser(),
        })
      );
      this.toast.show('Assessment published successfully', 'success');
      await this.loadAssessment(assessment.id);
    } catch (error: any) {
      this.logger.error('Failed to publish assessment', error);
      this.toast.show(
        error.error?.message || 'Failed to publish assessment',
        'error'
      );
    }
  }

  editAssessment(): void {
    const assessment = this.assessment();
    if (assessment) {
      this.router.navigate(['/admin/examination/assessments', assessment.id, 'edit']);
    }
  }

  goBack(): void {
    this.router.navigate(['/admin/examination/assessments']);
  }

  getStatusBadgeClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'draft':
        return 'badge--warning';
      case 'published':
        return 'badge--success';
      case 'completed':
        return 'badge--info';
      default:
        return 'badge--neutral';
    }
  }

  getTotalComponentWeightage(): number {
    const assessment = this.assessment();
    if (!assessment || !assessment.components) {
      return 0;
    }
    return assessment.components.reduce((sum, c) => sum + c.weightage, 0);
  }

  private getCurrentUser(): string {
    const profile = this.authService.profile;
    return profile?.fullName || profile?.email || profile?.uniqueId || 'System';
  }
}
