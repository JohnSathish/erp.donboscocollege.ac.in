import { Component, EventEmitter, inject, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { ToastService } from '../shared/toast.service';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
})
export class ChangePasswordComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);

  @Output() closed = new EventEmitter<void>();
  @Output() passwordUpdated = new EventEmitter<void>();

  isSubmitting = false;

  readonly form = this.fb.nonNullable.group({
    currentPassword: ['', [Validators.required, Validators.minLength(5)]], // Allow 5-digit temporary password
    newPassword: ['', [Validators.required, Validators.minLength(8)]], // New password must be stronger
    confirmPassword: ['', [Validators.required, Validators.minLength(8)]],
  });

  submit(): void {
    if (this.form.invalid || this.passwordsMismatch) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    const { currentPassword, newPassword } = this.form.getRawValue();

    this.auth.changePassword({ currentPassword, newPassword }).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.toast.show('Password updated successfully.', 'success');
        this.passwordUpdated.emit();
        this.router.navigate(['/app', 'dashboard']);
        this.close();
      },
      error: (error) => {
        this.isSubmitting = false;
        const message =
          error.status === 401
            ? 'Current password is incorrect or your session expired. Please log in again.'
            : 'We were unable to change your password. Please try again later.';
        this.toast.show(message, 'error');
      },
    });
  }

  cancel(): void {
    this.close();
  }

  private close(): void {
    this.form.reset();
    this.closed.emit();
  }

  get passwordsMismatch(): boolean {
    const { newPassword, confirmPassword } = this.form.getRawValue();
    return newPassword !== confirmPassword;
  }

  controlInvalid(control: string): boolean {
    const ctrl = this.form.get(control);
    return !!ctrl && ctrl.invalid && (ctrl.dirty || ctrl.touched);
  }
}
