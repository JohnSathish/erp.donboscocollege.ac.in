import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
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

    this.auth.login({ username, password }, rememberMe).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        if (response.mustChangePassword === true) {
          this.router.navigate(['/app', 'dashboard'], {
            queryParams: { forcePassword: true },
          });
        } else {
          this.router.navigate(['/app', 'dashboard']);
        }
      },
      error: (error) => {
        this.isSubmitting = false;
        if (error.status === 401) {
          this.serverError =
            'Invalid credentials or account temporarily locked. Please verify your details and try again.';
        } else {
          this.serverError =
            'We were unable to sign you in. Please try again shortly or contact admissions support.';
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
