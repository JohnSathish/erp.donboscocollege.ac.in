import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AdmissionsAdminApiService } from '@client/shared/data';
import {
  ExamRegistrationDto,
  ExamRegistrationsListResponse,
  EntranceExamDto,
  OnlineApplicationDto,
  OnlineApplicationsListResponse,
} from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-exam-registrations',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-exam-registrations.component.html',
  styleUrl: './admin-exam-registrations.component.scss',
})
export class AdminExamRegistrationsComponent implements OnInit {
  private readonly apiService = inject(AdmissionsAdminApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly toast: ToastService = inject(ToastService);

  exam = signal<EntranceExamDto | null>(null);
  registrations = signal<ExamRegistrationDto[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = signal(20);
  isLoading = signal(false);
  searchTerm = signal('');
  isPresentFilter = signal<boolean | null>(null);

  // Register Applicant Modal
  showRegisterModal = signal(false);
  applicantSearchTerm = signal('');
  availableApplicants = signal<OnlineApplicationDto[]>([]);
  applicantSearchLoading = signal(false);
  applicantSearchPage = signal(1);
  applicantSearchTotal = signal(0);
  registeringApplicantId = signal<string | null>(null);

  ngOnInit(): void {
    const examId = this.route.snapshot.paramMap.get('id');
    if (examId) {
      this.loadExam(examId);
      this.loadRegistrations(examId);
    }
  }

  loadExam(examId: string): void {
    this.apiService.getEntranceExam(examId).subscribe({
      next: (exam: EntranceExamDto) => {
        this.exam.set(exam);
      },
      error: (error) => {
        console.error('Error loading exam:', error);
        this.toast.error('Failed to load exam details');
      },
    });
  }

  loadRegistrations(examId: string): void {
    this.isLoading.set(true);
    this.apiService
      .listExamRegistrations(examId, {
        page: this.currentPage(),
        pageSize: this.pageSize(),
        isPresent: this.isPresentFilter() ?? undefined,
        searchTerm: this.searchTerm() || undefined,
      })
      .subscribe({
        next: (response: ExamRegistrationsListResponse) => {
          this.registrations.set(response.registrations);
          this.totalCount.set(response.totalCount);
          this.isLoading.set(false);
        },
        error: (error) => {
          console.error('Error loading registrations:', error);
          this.toast.error('Failed to load registrations');
          this.isLoading.set(false);
        },
      });
  }

  onSearch(): void {
    this.currentPage.set(1);
    const examId = this.route.snapshot.paramMap.get('id');
    if (examId) {
      this.loadRegistrations(examId);
    }
  }

  onFilterChange(): void {
    this.currentPage.set(1);
    const examId = this.route.snapshot.paramMap.get('id');
    if (examId) {
      this.loadRegistrations(examId);
    }
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    const examId = this.route.snapshot.paramMap.get('id');
    if (examId) {
      this.loadRegistrations(examId);
    }
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount() / this.pageSize());
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  }

  openRegisterModal(): void {
    this.showRegisterModal.set(true);
    this.applicantSearchTerm.set('');
    this.availableApplicants.set([]);
    this.searchApplicants();
  }

  closeRegisterModal(): void {
    this.showRegisterModal.set(false);
    this.applicantSearchTerm.set('');
    this.availableApplicants.set([]);
    this.registeringApplicantId.set(null);
  }

  async searchApplicants(): Promise<void> {
    this.applicantSearchLoading.set(true);
    try {
      const response = await firstValueFrom(
        this.apiService.listOnlineApplications({
          page: this.applicantSearchPage(),
          pageSize: 50,
          searchTerm: this.applicantSearchTerm() || undefined,
          isApplicationSubmitted: true, // Only show submitted applications
        })
      );
      this.availableApplicants.set(response.applications);
      this.applicantSearchTotal.set(response.totalCount);
    } catch (error) {
      console.error('Error searching applicants:', error);
      this.toast.error('Failed to search applicants');
    } finally {
      this.applicantSearchLoading.set(false);
    }
  }

  onApplicantSearch(): void {
    this.applicantSearchPage.set(1);
    this.searchApplicants();
  }

  isApplicantAlreadyRegistered(applicantId: string): boolean {
    return this.registrations().some(
      (reg) => reg.applicantAccountId === applicantId
    );
  }

  async registerApplicant(applicantId: string): Promise<void> {
    const examId = this.route.snapshot.paramMap.get('id');
    if (!examId) {
      this.toast.error('Exam ID not found');
      return;
    }

    if (this.isApplicantAlreadyRegistered(applicantId)) {
      this.toast.error('Applicant is already registered for this exam');
      return;
    }

    this.registeringApplicantId.set(applicantId);
    try {
      await firstValueFrom(
        this.apiService.registerApplicantForExam(examId, applicantId)
      );
      this.toast.success('Applicant registered successfully!');
      this.closeRegisterModal();
      // Reload registrations and exam to update counts
      this.loadRegistrations(examId);
      this.loadExam(examId);
    } catch (error: any) {
      console.error('Error registering applicant:', error);
      const errorMessage =
        error?.error?.message || error?.message || 'Failed to register applicant';
      this.toast.error(errorMessage);
    } finally {
      this.registeringApplicantId.set(null);
    }
  }
}

