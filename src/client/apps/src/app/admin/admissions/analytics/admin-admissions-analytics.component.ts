import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AdmissionsAdminApiService, AdmissionsAnalyticsDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

@Component({
  selector: 'app-admin-admissions-analytics',
  standalone: true,
  imports: [CommonModule, DatePipe, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './admin-admissions-analytics.component.html',
  styleUrls: ['./admin-admissions-analytics.component.scss'],
})
export class AdminAdmissionsAnalyticsComponent implements OnInit {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly toast = inject(ToastService);
  private readonly fb = inject(FormBuilder);

  protected readonly analytics = signal<AdmissionsAnalyticsDto | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);

  protected readonly dateRangeForm = this.fb.group({
    fromDate: [''],
    toDate: [''],
  });

  async ngOnInit(): Promise<void> {
    await this.loadAnalytics();
  }

  async loadAnalytics(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const formValue = this.dateRangeForm.getRawValue();
      const params: any = {};

      if (formValue.fromDate) {
        params.fromDate = new Date(formValue.fromDate).toISOString();
      }
      if (formValue.toDate) {
        params.toDate = new Date(formValue.toDate).toISOString();
      }

      const data = await firstValueFrom(this.api.getAdmissionsAnalytics(params));
      this.analytics.set(data);
    } catch (error: any) {
      console.error('Failed to load analytics', error);
      this.error.set('Failed to load analytics. Please try again.');
      this.toast.show('Failed to load analytics.', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async applyFilters(): Promise<void> {
    await this.loadAnalytics();
  }

  resetFilters(): void {
    this.dateRangeForm.reset();
    this.loadAnalytics();
  }

  getStatusPercentage(status: string): number {
    const analyticsData = this.analytics();
    if (!analyticsData) return 0;

    const total = analyticsData.statusDistribution.submitted +
                  analyticsData.statusDistribution.approved +
                  analyticsData.statusDistribution.rejected +
                  analyticsData.statusDistribution.waitingList +
                  analyticsData.statusDistribution.entranceExam;

    if (total === 0) return 0;

    const value = analyticsData.statusDistribution[status as keyof typeof analyticsData.statusDistribution] || 0;
    return (value / total) * 100;
  }

  getTrendBarHeight(count: number, type: 'paid' | 'pending'): number {
    const analyticsData = this.analytics();
    if (!analyticsData || analyticsData.paymentAnalytics.paymentTrend.length === 0) return 0;

    const maxCount = Math.max(
      ...analyticsData.paymentAnalytics.paymentTrend.map(t => Math.max(t.paidCount, t.pendingCount))
    );

    if (maxCount === 0) return 0;
    return Math.max((count / maxCount) * 150, 20); // Min height 20px, max 150px
  }

  getDailyBarHeight(count: number): number {
    const analyticsData = this.analytics();
    if (!analyticsData || analyticsData.dailyApplications.length === 0) return 0;

    const maxCount = Math.max(...analyticsData.dailyApplications.map(d => d.count));
    if (maxCount === 0) return 0;
    return Math.max((count / maxCount) * 200, 20); // Min height 20px, max 200px
  }

  // Expose Math for template
  Math = Math;
}

