import { Component, inject, OnInit, signal, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { AdmissionsAdminApiService } from '@client/shared/data';
import { EntranceExamDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';

@Component({
  selector: 'app-admin-entrance-exam-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './admin-entrance-exam-detail.component.html',
  styleUrls: ['./admin-entrance-exam-detail.component.scss'],
})
export class AdminEntranceExamDetailComponent implements OnInit {
  private readonly apiService = inject(AdmissionsAdminApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast: ToastService = inject(ToastService);

  exam = signal<EntranceExamDto | null>(null);
  isLoading = signal(false);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadExam(id);
    }
  }

  loadExam(id: string): void {
    this.isLoading.set(true);
    this.apiService.getEntranceExam(id).subscribe({
      next: (exam: EntranceExamDto) => {
        this.exam.set(exam);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading exam:', error);
        this.toast.error('Failed to load exam details');
        this.isLoading.set(false);
        this.router.navigate(['/admin/admissions/exams']);
      },
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  }

  formatTime(timeString: string): string {
    return timeString;
  }
}

