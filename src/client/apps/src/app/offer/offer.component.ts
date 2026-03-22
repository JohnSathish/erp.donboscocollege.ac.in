import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import {
  ApplicantApplicationApiService,
  AdmissionOfferDto,
  AcceptOfferResponse,
  RejectOfferResponse,
} from '@client/shared/data';
import { ApplicantPortalStore } from '../dashboard/applicant-portal.store';
import { ToastService } from '../shared/toast.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-offer',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './offer.component.html',
  styleUrls: ['./offer.component.scss'],
})
export class OfferComponent implements OnInit {
  private readonly api = inject(ApplicantApplicationApiService);
  private readonly store = inject(ApplicantPortalStore);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly loading = signal(false);
  readonly processing = signal(false);
  readonly offer = signal<AdmissionOfferDto | null>(null);
  readonly error = signal<string | null>(null);
  rejectReason = ''; // Regular property for two-way binding

  ngOnInit(): void {
    this.loadOffer();
  }

  loadOffer(): void {
    this.loading.set(true);
    this.error.set(null);

    this.api
      .getOffer()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (offer) => {
          this.offer.set(offer);
        },
        error: (error) => {
          if (error.status === 404) {
            this.offer.set(null);
          } else {
            this.error.set(
              error?.error?.message || 'Failed to load admission offer.'
            );
          }
        },
      });
  }

  acceptOffer(): void {
    if (
      !confirm(
        'Are you sure you want to accept this admission offer? This action cannot be undone.'
      )
    ) {
      return;
    }

    this.processing.set(true);
    this.error.set(null);

    this.api
      .acceptOffer()
      .pipe(finalize(() => this.processing.set(false)))
      .subscribe({
        next: (result: AcceptOfferResponse) => {
          if (result.success) {
            this.toast.show(result.message, 'success');
            this.loadOffer();
            this.store.loadDashboard();
          } else {
            this.toast.show(result.message, 'error');
          }
        },
        error: (error) => {
          const message =
            error?.error?.message || 'Failed to accept admission offer.';
          this.toast.show(message, 'error');
          this.error.set(message);
        },
      });
  }

  rejectOffer(): void {
    if (
      !confirm(
        'Are you sure you want to reject this admission offer? This action cannot be undone.'
      )
    ) {
      return;
    }

    this.processing.set(true);
    this.error.set(null);

    this.api
      .rejectOffer(this.rejectReason || undefined)
      .pipe(finalize(() => this.processing.set(false)))
      .subscribe({
        next: (result: RejectOfferResponse) => {
          if (result.success) {
            this.toast.show(result.message, 'success');
            this.loadOffer();
            this.store.loadDashboard();
          } else {
            this.toast.show(result.message, 'error');
          }
        },
        error: (error) => {
          const message =
            error?.error?.message || 'Failed to reject admission offer.';
          this.toast.show(message, 'error');
          this.error.set(message);
        },
      });
  }

  isExpired(expiryDate: string): boolean {
    return new Date(expiryDate) < new Date();
  }

  daysUntilExpiry(expiryDate: string): number {
    const expiry = new Date(expiryDate);
    const now = new Date();
    const diff = expiry.getTime() - now.getTime();
    return Math.ceil(diff / (1000 * 60 * 60 * 24));
  }

  /** Primary CTA — Razorpay flow lives on dashboard payment section. */
  goToAdmissionFeePayment(): void {
    void this.router.navigate(['/app/dashboard'], { fragment: 'payment' });
  }

  /** Opens print dialog; use “Save as PDF” in the browser where available. */
  printOfferLetter(): void {
    window.print();
  }
}

