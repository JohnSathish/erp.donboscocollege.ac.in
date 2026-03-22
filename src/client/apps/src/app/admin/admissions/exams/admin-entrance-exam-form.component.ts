import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { AdmissionsAdminApiService } from '@client/shared/data';
import { EntranceExamDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';

@Component({
  selector: 'app-admin-entrance-exam-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './admin-entrance-exam-form.component.html',
  styleUrl: './admin-entrance-exam-form.component.scss',
})
export class AdminEntranceExamFormComponent implements OnInit {
  private readonly apiService = inject(AdmissionsAdminApiService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly fb = inject(FormBuilder);
  private readonly toast: ToastService = inject(ToastService);

  examForm!: FormGroup;
  isEditMode = signal(false);
  examId = signal<string | null>(null);
  isLoading = signal(false);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && this.route.snapshot.url.some(segment => segment.path === 'edit')) {
      this.isEditMode.set(true);
      this.examId.set(id);
      this.loadExam(id);
    }
    this.initializeForm();
  }

  initializeForm(): void {
    const now = new Date();
    const defaultStartTime = '09:00';
    const defaultEndTime = '12:00';

    this.examForm = this.fb.group({
      examName: ['', [Validators.required, Validators.maxLength(256)]],
      examCode: ['', [Validators.required, Validators.maxLength(32)]],
      examDate: ['', Validators.required],
      examStartTime: [defaultStartTime, Validators.required],
      examEndTime: [defaultEndTime, Validators.required],
      venue: ['', [Validators.required, Validators.maxLength(256)]],
      venueAddress: ['', Validators.maxLength(500)],
      maxCapacity: [100, [Validators.required, Validators.min(1)]],
      description: ['', Validators.maxLength(1000)],
      instructions: ['', Validators.maxLength(2000)],
    });

    if (this.isEditMode()) {
      this.examForm.get('examCode')?.disable();
    }
  }

  loadExam(id: string): void {
    this.isLoading.set(true);
    this.apiService.getEntranceExam(id).subscribe({
      next: (exam: EntranceExamDto) => {
        const examDate = new Date(exam.examDate);
        const dateStr = examDate.toISOString().split('T')[0];
        
        // Ensure examCode is disabled in edit mode
        if (this.isEditMode()) {
          this.examForm.get('examCode')?.disable();
        }
        
        this.examForm.patchValue({
          examName: exam.examName,
          examCode: exam.examCode,
          examDate: dateStr,
          examStartTime: exam.examStartTime,
          examEndTime: exam.examEndTime,
          venue: exam.venue,
          venueAddress: exam.venueAddress || '',
          maxCapacity: exam.maxCapacity,
          description: exam.description || '',
          instructions: exam.instructions || '',
        });
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading exam:', error);
        this.toast.error('Failed to load exam');
        this.isLoading.set(false);
      },
    });
  }

  onSubmit(): void {
    if (this.examForm.invalid) {
      this.examForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    const formValue = this.examForm.getRawValue();

    if (this.isEditMode() && this.examId()) {
      const updatePayload = {
        examName: formValue.examName,
        examDate: formValue.examDate,
        examStartTime: formValue.examStartTime,
        examEndTime: formValue.examEndTime,
        venue: formValue.venue,
        maxCapacity: formValue.maxCapacity,
        description: formValue.description || null,
        venueAddress: formValue.venueAddress || null,
        instructions: formValue.instructions || null,
      };

      this.apiService.updateEntranceExam(this.examId()!, updatePayload).subscribe({
        next: () => {
          this.toast.success('Exam updated successfully');
          this.router.navigate(['/admin/admissions/exams']);
        },
        error: (error) => {
          console.error('Error updating exam:', error);
          this.toast.error('Failed to update exam');
          this.isLoading.set(false);
        },
      });
    } else {
      const createPayload = {
        examName: formValue.examName,
        examCode: formValue.examCode.toUpperCase(),
        examDate: formValue.examDate,
        examStartTime: formValue.examStartTime,
        examEndTime: formValue.examEndTime,
        venue: formValue.venue,
        maxCapacity: formValue.maxCapacity,
        description: formValue.description || null,
        venueAddress: formValue.venueAddress || null,
        instructions: formValue.instructions || null,
      };

      this.apiService.createEntranceExam(createPayload).subscribe({
        next: () => {
          this.toast.success('Exam created successfully');
          this.router.navigate(['/admin/admissions/exams']);
        },
        error: (error) => {
          console.error('Error creating exam:', error);
          const errorMessage = (error as any)?.error?.message || 'Failed to create exam';
          this.toast.error(errorMessage);
          this.isLoading.set(false);
        },
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/admin/admissions/exams']);
  }
}

