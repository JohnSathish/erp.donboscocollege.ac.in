import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AdmissionsAdminApiService, PaymentDto, PaymentsListResponse, PaymentReportDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

@Component({
  selector: 'app-admin-payment-management',
  standalone: true,
  imports: [CommonModule, RouterModule, DatePipe, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './admin-payment-management.component.html',
  styleUrls: ['./admin-payment-management.component.scss'],
})
export class AdminPaymentManagementComponent implements OnInit {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly toast = inject(ToastService);
  private readonly fb = inject(FormBuilder);

  protected readonly payments = signal<PaymentDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly totalCount = signal(0);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(50);
  protected readonly filtersOpen = signal(false);
  protected readonly reportOpen = signal(false);
  protected readonly reportLoading = signal(false);
  protected readonly report = signal<PaymentReportDto | null>(null);

  protected readonly filterForm = this.fb.nonNullable.group({
    isPaymentCompleted: [null as boolean | null],
    searchTerm: [''],
    paymentDateFrom: [''],
    paymentDateTo: [''],
    minAmount: [null as number | null],
    maxAmount: [null as number | null],
  });

  protected readonly totalRevenue = computed(() =>
    this.payments()
      .filter(p => p.isPaymentCompleted && p.paymentAmount)
      .reduce((sum, p) => sum + (p.paymentAmount || 0), 0)
  );

  protected readonly paidCount = computed(() =>
    this.payments().filter(p => p.isPaymentCompleted).length
  );

  protected readonly unpaidCount = computed(() =>
    this.payments().filter(p => !p.isPaymentCompleted).length
  );

  async ngOnInit(): Promise<void> {
    await this.loadPayments();
  }

  async loadPayments(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const formValue = this.filterForm.getRawValue();
      const params: any = {
        page: this.currentPage(),
        pageSize: this.pageSize(),
      };

      if (formValue.isPaymentCompleted !== null) {
        params.isPaymentCompleted = formValue.isPaymentCompleted;
      }
      if (formValue.searchTerm?.trim()) {
        params.searchTerm = formValue.searchTerm.trim();
      }
      if (formValue.paymentDateFrom) {
        params.paymentDateFrom = new Date(formValue.paymentDateFrom).toISOString();
      }
      if (formValue.paymentDateTo) {
        params.paymentDateTo = new Date(formValue.paymentDateTo).toISOString();
      }
      if (formValue.minAmount !== null) {
        params.minAmount = formValue.minAmount;
      }
      if (formValue.maxAmount !== null) {
        params.maxAmount = formValue.maxAmount;
      }

      const response = await firstValueFrom(this.api.listPayments(params));
      this.payments.set(response.payments);
      this.totalCount.set(response.totalCount);
    } catch (error: any) {
      console.error('Failed to load payments', error);
      this.error.set('Failed to load payments. Please try again.');
      this.toast.show('Failed to load payments.', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onPageChange(page: number): Promise<void> {
    this.currentPage.set(page);
    await this.loadPayments();
  }

  async applyFilters(): Promise<void> {
    this.currentPage.set(1);
    await this.loadPayments();
    this.filtersOpen.set(false);
  }

  async clearFilters(): Promise<void> {
    this.filterForm.reset({
      isPaymentCompleted: null,
      searchTerm: '',
      paymentDateFrom: '',
      paymentDateTo: '',
      minAmount: null,
      maxAmount: null,
    });
    this.currentPage.set(1);
    await this.loadPayments();
  }

  toggleFilters(): void {
    this.filtersOpen.set(!this.filtersOpen());
  }

  getStatusBadgeClass(payment: PaymentDto): string {
    return payment.isPaymentCompleted ? 'badge-success' : 'badge-warning';
  }

  getStatusText(payment: PaymentDto): string {
    return payment.isPaymentCompleted ? 'Paid' : 'Pending';
  }

  async generateReport(): Promise<void> {
    this.reportOpen.set(true);
    this.reportLoading.set(true);
    this.report.set(null);

    try {
      const formValue = this.filterForm.getRawValue();
      const params: any = {};

      if (formValue.isPaymentCompleted !== null) {
        params.isPaymentCompleted = formValue.isPaymentCompleted;
      }
      if (formValue.paymentDateFrom) {
        params.fromDate = new Date(formValue.paymentDateFrom).toISOString();
      }
      if (formValue.paymentDateTo) {
        params.toDate = new Date(formValue.paymentDateTo).toISOString();
      }

      const reportData = await firstValueFrom(this.api.getPaymentReport(params));
      this.report.set(reportData);
    } catch (error: any) {
      console.error('Failed to generate report', error);
      this.toast.show('Failed to generate payment report.', 'error');
    } finally {
      this.reportLoading.set(false);
    }
  }

  closeReport(): void {
    this.reportOpen.set(false);
    this.report.set(null);
  }

  async exportReportToExcel(): Promise<void> {
    const reportData = this.report();
    if (!reportData) {
      return;
    }

    try {
      // Use the existing export service for paid applications
      // For now, we'll export all paid applications
      const blob = await firstValueFrom(this.api.exportPaidApplicationsWithFullDetails());
      const url = window.URL.createObjectURL(blob);
      const link = window.document.createElement('a');
      link.href = url;
      link.download = `payment-report-${new Date().toISOString().slice(0, 10)}.xlsx`;
      window.document.body.appendChild(link);
      link.click();
      window.document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
      this.toast.show('Report exported successfully!', 'success');
    } catch (error: any) {
      console.error('Failed to export report', error);
      this.toast.show('Failed to export report.', 'error');
    }
  }

  // Expose Math for template
  Math = Math;
}

