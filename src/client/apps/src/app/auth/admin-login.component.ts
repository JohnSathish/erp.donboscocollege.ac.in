import { Component, inject, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss'],
})
export class AdminLoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  serverError: string | null = null;
  isSubmitting = false;
  showPassword = false;

  readonly form = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.maxLength(256)]],
    password: ['', [Validators.required, Validators.minLength(5)]],
    rememberMe: [false],
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.serverError = null;

    const { username, password, rememberMe } = this.form.getRawValue();

    this.auth.loginAdmin({ username, password }, rememberMe).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/admin', 'dashboard']);
      },
      error: (error) => {
        this.isSubmitting = false;
        console.error('Login error:', error);
        
        if (error.status === 401) {
          this.serverError =
            'Invalid credentials. Please verify your username and password.';
        } else if (error.status === 0 || error.status === undefined) {
          this.serverError =
            'Unable to connect to server. Please ensure the backend API is running on http://localhost:5227';
        } else if (error.status >= 500) {
          this.serverError =
            'Server error. Please try again later or contact system administrator.';
        } else {
          const errorMessage = error?.error?.message || error?.message || 'Unknown error';
          this.serverError =
            `Unable to sign in: ${errorMessage}. Please try again or contact system administrator.`;
        }
      },
    });
  }

  controlInvalid(control: string): boolean {
    const ctrl = this.form.get(control);
    return !!ctrl && ctrl.invalid && (ctrl.dirty || ctrl.touched);
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
}

