import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AdmissionsAdminApiService } from '@client/shared/data';
import {
  EntranceExamDto,
  EntranceExamsListResponse,
} from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';

@Component({
  selector: 'app-admin-entrance-exams-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-entrance-exams-list.component.html',
  styleUrl: './admin-entrance-exams-list.component.scss',
})
export class AdminEntranceExamsListComponent implements OnInit {
  private readonly apiService = inject(AdmissionsAdminApiService);
  private readonly toast: ToastService = inject(ToastService);

  exams = signal<EntranceExamDto[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = signal(20);
  isLoading = signal(false);
  searchTerm = signal('');
  isActiveFilter = signal<boolean | null>(null);

  ngOnInit(): void {
    this.loadExams();
  }

  loadExams(): void {
    this.isLoading.set(true);
    this.apiService
      .listEntranceExams({
        page: this.currentPage(),
        pageSize: this.pageSize(),
        isActive: this.isActiveFilter() ?? undefined,
        searchTerm: this.searchTerm() || undefined,
      })
      .subscribe({
        next: (response: EntranceExamsListResponse) => {
          this.exams.set(response.exams);
          this.totalCount.set(response.totalCount);
          this.isLoading.set(false);
        },
        error: (error) => {
          console.error('Error loading exams:', error);
          this.toast.error('Failed to load exams');
          this.isLoading.set(false);
        },
      });
  }

  onSearch(): void {
    this.currentPage.set(1);
    this.loadExams();
  }

  onFilterChange(): void {
    this.currentPage.set(1);
    this.loadExams();
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadExams();
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount() / this.pageSize());
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }

  formatTime(timeString: string): string {
    // Assuming timeString is in HH:mm format
    return timeString;
  }

  toggleExamStatus(exam: EntranceExamDto): void {
    // This would require an API endpoint to toggle status
    // For now, we'll just show a message
    this.toast.info('Status toggle functionality coming soon');
  }
}

