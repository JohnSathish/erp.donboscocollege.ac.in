import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import {
  RegisterStudentApplicantRequest,
  RegistrationApiService,
} from '@client/shared/data';

interface RegistrationSuccess {
  uniqueId: string;
  temporaryPassword: string;
}

const MAX_PHOTO_BYTES = 2 * 1024 * 1024;
const ACCEPT_MIME = new Set(['image/jpeg', 'image/png']);

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
})
export class RegistrationComponent {
  @ViewChild('photoInput') photoInput?: ElementRef<HTMLInputElement>;

  readonly genderOptions = ['Female', 'Male', 'Other', 'Prefer not to say'];
  readonly maxDate: string = new Date().toISOString().split('T')[0];
  /** Display label for the current admission cycle (update yearly). */
  readonly admissionCycleLabel = '2026–2027';

  isSubmitting = false;
  submitError: string | null = null;
  success: RegistrationSuccess | null = null;

  photoFile: File | null = null;
  photoPreviewUrl: string | null = null;
  photoError: string | null = null;
  isDragActive = false;

  readonly form = this.fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.maxLength(256)]],
    dateOfBirth: ['', Validators.required],
    gender: ['', Validators.required],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    mobileNumber: [
      '',
      [Validators.required, Validators.pattern(/^[0-9+\-\s]{8,15}$/)],
    ],
    agreedToTerms: [false, Validators.requiredTrue],
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly api: RegistrationApiService
  ) {}

  openPhotoPicker(): void {
    this.photoInput?.nativeElement.click();
  }

  onPhotoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.applyPhotoFile(input.files?.[0] ?? null);
    input.value = '';
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragActive = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragActive = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragActive = false;
    const file = event.dataTransfer?.files?.[0] ?? null;
    this.applyPhotoFile(file);
  }

  private applyPhotoFile(file: File | null): void {
    this.photoError = null;
    if (this.photoPreviewUrl) {
      URL.revokeObjectURL(this.photoPreviewUrl);
      this.photoPreviewUrl = null;
    }
    this.photoFile = null;

    if (!file) {
      return;
    }
    if (!ACCEPT_MIME.has(file.type)) {
      this.photoError = 'Please upload a JPEG or PNG image.';
      return;
    }
    if (file.size > MAX_PHOTO_BYTES) {
      this.photoError = 'Photo must be 2 MB or smaller.';
      return;
    }
    this.photoFile = file;
    this.photoPreviewUrl = URL.createObjectURL(file);
  }

  clearPhoto(): void {
    this.applyPhotoFile(null);
  }

  submit(): void {
    if (!this.photoFile) {
      this.photoError = 'Please upload a passport-style photograph.';
    }
    if (this.form.invalid || !this.photoFile) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.submitError = null;
    this.success = null;

    const payload = this.mapFormValues();

    this.api
      .registerApplicant(payload, this.photoFile!)
      .pipe(finalize(() => (this.isSubmitting = false)))
      .subscribe({
        next: (response) => {
          this.success = {
            uniqueId: response.uniqueId,
            temporaryPassword: response.temporaryPassword,
          };
          this.form.reset();
          this.form.patchValue({ agreedToTerms: false });
          this.clearPhoto();
        },
        error: (error) => {
          if (error?.error?.message) {
            this.submitError = error.error.message;
            return;
          }

          this.submitError =
            'We were unable to process your registration at this time. Please try again in a few minutes.';
        },
      });
  }

  private mapFormValues(): RegisterStudentApplicantRequest {
    const value = this.form.getRawValue();
    return {
      fullName: value.fullName.trim(),
      dateOfBirth: value.dateOfBirth,
      gender: value.gender,
      email: value.email.trim(),
      mobileNumber: value.mobileNumber.trim(),
    };
  }

  controlInvalid(control: string): boolean {
    const ctrl = this.form.get(control);
    return !!ctrl && ctrl.invalid && (ctrl.dirty || ctrl.touched);
  }
}
