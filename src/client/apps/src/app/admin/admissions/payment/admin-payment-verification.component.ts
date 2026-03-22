import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, DatePipe, CurrencyPipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AdmissionsAdminApiService, PaymentDto, PaymentsListResponse } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

@Component({
  selector: 'app-admin-payment-verification',
  standalone: true,
  imports: [CommonModule, RouterModule, DatePipe, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './admin-payment-verification.component.html',
  styleUrls: ['./admin-payment-verification.component.scss'],
})
export class AdminPaymentVerificationComponent implements OnInit {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly toast = inject(ToastService);
  private readonly fb = inject(FormBuilder);

  protected readonly payments = signal<PaymentDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly verifying = signal<string | null>(null);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(50);
  protected readonly totalCount = signal(0);

  protected readonly verifyForm = this.fb.nonNullable.group({
    transactionId: ['', [Validators.required, Validators.minLength(3)]],
    amount: [0, [Validators.required, Validators.min(1)]],
    remarks: [''],
  });

  protected readonly pendingPayments = computed(() =>
    this.payments().filter(p => !p.isPaymentCompleted)
  );

  async ngOnInit(): Promise<void> {
    await this.loadPendingPayments();
  }

  async loadPendingPayments(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const response = await firstValueFrom(
        this.api.listPayments({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          isPaymentCompleted: false, // Only pending payments
        })
      );
      this.payments.set(response.payments);
      this.totalCount.set(response.totalCount);
    } catch (error: any) {
      console.error('Failed to load pending payments', error);
      this.error.set('Failed to load pending payments. Please try again.');
      this.toast.show('Failed to load pending payments.', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  openVerifyModal(payment: PaymentDto): void {
    this.verifying.set(payment.accountId);
    this.verifyForm.reset({
      transactionId: '',
      amount: 600, // Default application fee
      remarks: '',
    });
  }

  closeVerifyModal(): void {
    this.verifying.set(null);
    this.verifyForm.reset();
  }

  async verifyPayment(): Promise<void> {
    if (this.verifyForm.invalid) {
      this.verifyForm.markAllAsTouched();
      return;
    }

    const accountId = this.verifying();
    if (!accountId) {
      return;
    }

    const payment = this.payments().find(p => p.accountId === accountId);
    if (!payment) {
      this.toast.show('Payment not found.', 'error');
      return;
    }

    const formValue = this.verifyForm.getRawValue();

    try {
      await firstValueFrom(
        this.api.verifyPaymentManually(payment.accountId, {
          transactionId: formValue.transactionId,
          amount: formValue.amount,
          remarks: formValue.remarks || null,
        })
      );

      this.toast.show('Payment verified successfully!', 'success');
      this.closeVerifyModal();
      await this.loadPendingPayments();
    } catch (error: any) {
      console.error('Failed to verify payment', error);
      const errorMessage =
        error.error?.message || 'Failed to verify payment. Please try again.';
      this.toast.show(errorMessage, 'error');
    }
  }
}
