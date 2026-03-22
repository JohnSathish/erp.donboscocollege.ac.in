import { Injectable, signal } from '@angular/core';

export type ToastType = 'success' | 'error' | 'info';

export interface ToastMessage {
  id: number;
  message: string;
  type: ToastType;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private readonly toastsSignal = signal<ToastMessage[]>([]);
  private nextId = 1;

  readonly toasts = this.toastsSignal.asReadonly();

  show(message: string, type: ToastType = 'info', duration = 4000): void {
    const toast: ToastMessage = {
      id: this.nextId++,
      message,
      type,
    };

    this.toastsSignal.update((current) => [...current, toast]);

    if (duration > 0) {
      window.setTimeout(() => this.dismiss(toast.id), duration);
    }
  }

  dismiss(id: number): void {
    this.toastsSignal.update((current) => current.filter((toast) => toast.id !== id));
  }

  clear(): void {
    this.toastsSignal.set([]);
  }

  success(message: string, duration = 4000): void {
    this.show(message, 'success', duration);
  }

  error(message: string, duration = 5000): void {
    this.show(message, 'error', duration);
  }

  info(message: string, duration = 4000): void {
    this.show(message, 'info', duration);
  }
}
