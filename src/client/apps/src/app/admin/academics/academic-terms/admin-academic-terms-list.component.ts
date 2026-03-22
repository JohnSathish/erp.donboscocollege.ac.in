import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TimetableApiService, CreateAcademicTermPayload, AcademicTermDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-academic-terms-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, DatePipe],
  templateUrl: './admin-academic-terms-list.component.html',
  styleUrls: ['./admin-academic-terms-list.component.scss'],
})
export class AdminAcademicTermsListComponent implements OnInit {
  private readonly api = inject(TimetableApiService);
  private readonly toast = inject(ToastService);

  terms = signal<AcademicTermDto[]>([]);
  isLoading = signal(false);
  showCreateForm = signal(false);
  academicYearFilter = signal('');
  isActiveFilter = signal<boolean | null>(null);

  // Form fields
  termName = signal('');
  academicYear = signal('');
  startDate = signal('');
  endDate = signal('');
  remarks = signal('');

  ngOnInit(): void {
    this.loadTerms();
  }

  async loadTerms(): Promise<void> {
    this.isLoading.set(true);
    try {
      console.log('Loading academic terms...');
      const response = await firstValueFrom(
        this.api.listAcademicTerms({
          academicYear: this.academicYearFilter() || undefined,
          isActive: this.isActiveFilter() ?? undefined,
        })
      );
      console.log('Academic terms loaded:', response);
      this.terms.set(response);
    } catch (error: any) {
      console.error('Error loading academic terms:', error);
      console.error('Error status:', error?.status);
      console.error('Error message:', error?.message);
      console.error('Error details:', error?.error);
      
      let errorMessage = 'Failed to load academic terms';
      
      if (error?.status === 0) {
        errorMessage = 'Network error. Please check if the API server is running at http://localhost:5227';
      } else if (error?.status === 401) {
        errorMessage = 'Unauthorized. Please log in again.';
      } else if (error?.status === 404) {
        errorMessage = 'Endpoint not found. The API endpoint may not be available.';
      } else if (error?.status === 500) {
        errorMessage = 'Server error. Please check the server logs.';
      } else if (error?.error?.message) {
        errorMessage = error.error.message;
      } else if (error?.message) {
        errorMessage = error.message;
      }
      
      this.toast.error(errorMessage);
    } finally {
      this.isLoading.set(false);
    }
  }

  onFilterChange(): void {
    this.loadTerms();
  }

  toggleCreateForm(): void {
    this.showCreateForm.set(!this.showCreateForm());
    if (this.showCreateForm()) {
      this.resetForm();
    }
  }

  resetForm(): void {
    this.termName.set('');
    this.academicYear.set('');
    this.startDate.set('');
    this.endDate.set('');
    this.remarks.set('');
  }

  async createTerm(): Promise<void> {
    if (!this.termName() || !this.academicYear() || !this.startDate() || !this.endDate()) {
      this.toast.error('Please fill in all required fields');
      return;
    }

    if (new Date(this.endDate()) <= new Date(this.startDate())) {
      this.toast.error('End date must be after start date');
      return;
    }

    this.isLoading.set(true);
    try {
      const payload: CreateAcademicTermPayload = {
        termName: this.termName(),
        academicYear: this.academicYear(),
        startDate: new Date(this.startDate()).toISOString(),
        endDate: new Date(this.endDate()).toISOString(),
        remarks: this.remarks() || undefined,
      };

      await firstValueFrom(this.api.createAcademicTerm(payload));
      this.toast.success('Academic term created successfully');
      this.toggleCreateForm();
      await this.loadTerms();
    } catch (error: any) {
      console.error('Error creating academic term:', error);
      const errorMessage = error?.error?.message || error?.message || 'Failed to create academic term';
      this.toast.error(errorMessage);
    } finally {
      this.isLoading.set(false);
    }
  }

  async toggleTermStatus(term: AcademicTermDto): Promise<void> {
    this.isLoading.set(true);
    try {
      if (term.isActive) {
        await firstValueFrom(this.api.deactivateAcademicTerm(term.id));
        this.toast.success('Academic term deactivated');
      } else {
        await firstValueFrom(this.api.activateAcademicTerm(term.id));
        this.toast.success('Academic term activated');
      }
      await this.loadTerms();
    } catch (error: any) {
      console.error('Error toggling term status:', error);
      this.toast.error('Failed to update term status');
    } finally {
      this.isLoading.set(false);
    }
  }
}

