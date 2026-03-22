import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AdmissionsAdminApiService } from '@client/shared/data';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-admin-dashboard-new',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './admin-dashboard-new.component.html',
  styleUrl: './admin-dashboard-new.component.scss',
})
export class AdminDashboardNewComponent implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly toast = inject(ToastService);
  
  exporting = signal(false);

  // Date filter
  dateRange = signal<'today' | 'week' | 'month' | 'year'>('month');
  
  // Dashboard data
  totalApplications = signal(2);
  submittedApplications = signal(1);
  approvedApplications = signal(1);
  pendingApplications = signal(0);
  totalRevenue = signal(10000);

  // Computed values
  approvalRate = computed(() => {
    const total = this.totalApplications();
    return total > 0 ? Math.round((this.approvedApplications() / total) * 100) : 0;
  });

  submissionRate = computed(() => {
    const total = this.totalApplications();
    return total > 0 ? Math.round((this.submittedApplications() / total) * 100) : 0;
  });

  // Recent applications (mock data)
  recentApplications = signal([
    { id: 1, name: 'John Doe', course: 'B.Sc Computer Science', status: 'Approved', date: '2025-01-20' },
    { id: 2, name: 'Jane Smith', course: 'B.A English', status: 'Pending', date: '2025-01-19' },
  ]);

  // Status breakdown
  statusBreakdown = computed(() => ({
    approved: this.approvedApplications(),
    pending: this.pendingApplications(),
    submitted: this.submittedApplications(),
    total: this.totalApplications(),
  }));

  // Expose Math for template
  Math = Math;

  setDateRange(range: 'today' | 'week' | 'month' | 'year') {
    this.dateRange.set(range);
    // TODO: Fetch data based on date range
  }

  ngOnInit() {
    // TODO: Load dashboard data from API
    this.loadDashboardData();
  }

  private loadDashboardData() {
    // TODO: Replace with actual API call
    // this.http.get('/api/admin/dashboard').subscribe(data => { ... });
  }

  async exportPaidApplications(): Promise<void> {
    if (this.exporting()) {
      return;
    }

    this.exporting.set(true);
    try {
      this.api.exportPaidApplicationsWithFullDetails().subscribe({
        next: (blob) => {
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url;
          link.download = `paid-applications-${new Date().toISOString().slice(0, 10)}.xlsx`;
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
          window.URL.revokeObjectURL(url);
          this.toast.show('Excel export downloaded successfully!', 'success');
          this.exporting.set(false);
        },
        error: (error) => {
          console.error('Export error:', error);
          this.toast.show('Failed to export applications. Please try again.', 'error');
          this.exporting.set(false);
        }
      });
    } catch (error) {
      console.error('Export error:', error);
      this.toast.show('Failed to export applications. Please try again.', 'error');
      this.exporting.set(false);
    }
  }
}

