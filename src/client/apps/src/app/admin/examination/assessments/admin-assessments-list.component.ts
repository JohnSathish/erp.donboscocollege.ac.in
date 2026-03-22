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
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ExaminationsApiService, AssessmentSummaryDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-assessments-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-assessments-list.component.html',
  styleUrls: ['./admin-assessments-list.component.scss'],
})
export class AdminAssessmentsListComponent implements OnInit {
  private readonly api = inject(ExaminationsApiService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly assessments = signal<AssessmentSummaryDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);

  protected readonly searchControl = this.fb.control<string>('');
  protected readonly statusFilterControl = this.fb.control<string>('');

  async ngOnInit(): Promise<void> {
    await this.loadAssessments();
  }

  async loadAssessments(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const status = this.statusFilterControl.value || undefined;
      const assessments = await firstValueFrom(
        this.api.listAssessments({ status })
      );
      
      // Apply search filter if any
      let filtered = assessments;
      const searchTerm = this.searchControl.value?.toLowerCase();
      if (searchTerm) {
        filtered = assessments.filter(
          (a) =>
            a.name.toLowerCase().includes(searchTerm) ||
            a.code.toLowerCase().includes(searchTerm) ||
            a.courseName?.toLowerCase().includes(searchTerm)
        );
      }
      
      this.assessments.set(filtered);
    } catch (error) {
      console.error('Failed to load assessments', error);
      this.error.set('Failed to load assessments. Please try again.');
      this.toast.show('Failed to load assessments', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    await this.loadAssessments();
  }

  async onStatusFilterChange(): Promise<void> {
    await this.loadAssessments();
  }

  async publishAssessment(assessmentId: string): Promise<void> {
    try {
      await firstValueFrom(this.api.publishAssessment(assessmentId));
      this.toast.show('Assessment published successfully', 'success');
      await this.loadAssessments();
    } catch (error) {
      console.error('Failed to publish assessment', error);
      this.toast.show('Failed to publish assessment', 'error');
    }
  }

  viewAssessment(assessmentId: string): void {
    this.router.navigate(['/admin/examination/assessments', assessmentId]);
  }

  createAssessment(): void {
    this.router.navigate(['/admin/examination/assessments/new']);
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
}




