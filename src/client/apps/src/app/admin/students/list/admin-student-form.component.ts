import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { StudentsApiService, StudentDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { LoggingService } from '../../../shared/logging.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-student-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-student-form.component.html',
  styleUrls: ['./admin-student-form.component.scss'],
})
export class AdminStudentFormComponent implements OnInit {
  private readonly api = inject(StudentsApiService);
  protected readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);
  private readonly logging = inject(LoggingService);

  protected readonly loading = signal(false);
  protected readonly isEditMode = signal(false);
  protected readonly studentId = signal<string | null>(null);

  protected readonly studentForm = this.fb.group({
    fullName: ['', [Validators.required, Validators.maxLength(256)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    mobileNumber: ['', [Validators.required, Validators.maxLength(32)]],
    shift: ['', [Validators.required]],
    programCode: ['', [Validators.maxLength(50)]],
    majorSubject: ['', [Validators.maxLength(100)]],
    minorSubject: ['', [Validators.maxLength(100)]],
    photoUrl: ['', [Validators.maxLength(500)]],
  });

  async ngOnInit(): Promise<void> {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode.set(true);
      this.studentId.set(id);
      await this.loadStudent(id);
    }
  }

  async loadStudent(id: string): Promise<void> {
    this.loading.set(true);
    try {
      const student = await firstValueFrom(this.api.getStudent(id));
      this.populateForm(student);
    } catch (error: any) {
      this.logging.error('Failed to load student', error);
      this.toast.show('Failed to load student details', 'error');
      this.router.navigate(['/admin/students/list']);
    } finally {
      this.loading.set(false);
    }
  }

  private populateForm(student: StudentDto): void {
    this.studentForm.patchValue({
      fullName: student.fullName,
      email: student.email,
      mobileNumber: student.mobileNumber,
      shift: student.shift,
      programCode: student.programCode || '',
      majorSubject: student.majorSubject || '',
      minorSubject: student.minorSubject || '',
      photoUrl: student.photoUrl || '',
    });
  }

  async onSubmit(): Promise<void> {
    if (this.studentForm.invalid) {
      this.studentForm.markAllAsTouched();
      this.toast.show('Please correct the form errors', 'error');
      return;
    }

    if (!this.isEditMode() || !this.studentId()) {
      this.toast.show('Student can only be edited, not created through this form', 'error');
      return;
    }

    this.loading.set(true);
    try {
      const formValue = this.studentForm.getRawValue();
      
      await firstValueFrom(
        this.api.updateStudentProfile(this.studentId()!, {
          fullName: formValue.fullName!,
          email: formValue.email!,
          mobileNumber: formValue.mobileNumber!,
          shift: formValue.shift!,
          programCode: formValue.programCode || null,
          majorSubject: formValue.majorSubject || null,
          minorSubject: formValue.minorSubject || null,
          photoUrl: formValue.photoUrl || null,
        })
      );
      
      this.toast.show('Student profile updated successfully', 'success');
      this.router.navigate(['/admin/students', this.studentId()]);
    } catch (error: any) {
      this.logging.error('Failed to save student', error);
      this.toast.show(
        error?.error?.message || 'Failed to save student. Please try again.',
        'error'
      );
    } finally {
      this.loading.set(false);
    }
  }

  onCancel(): void {
    if (this.studentId()) {
      this.router.navigate(['/admin/students', this.studentId()]);
    } else {
      this.router.navigate(['/admin/students/list']);
    }
  }

  getFieldError(fieldName: string): string | null {
    const control = this.studentForm.get(fieldName);
    if (control && control.invalid && control.touched) {
      if (control.errors?.['required']) {
        return `${this.getFieldLabel(fieldName)} is required`;
      }
      if (control.errors?.['email']) {
        return 'Please enter a valid email address';
      }
      if (control.errors?.['maxlength']) {
        const maxLength = control.errors['maxlength'].requiredLength;
        return `Maximum length is ${maxLength} characters`;
      }
    }
    return null;
  }

  private getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      fullName: 'Full Name',
      email: 'Email',
      mobileNumber: 'Mobile Number',
      shift: 'Shift',
      programCode: 'Program Code',
      majorSubject: 'Major Subject',
      minorSubject: 'Minor Subject',
      photoUrl: 'Photo URL',
    };
    return labels[fieldName] || fieldName;
  }
}




