import { Injectable, signal } from '@angular/core';

export interface ApplicantApplicationStep {
  index: number;
  title: string;
  description: string;
}

@Injectable({ providedIn: 'root' })
export class ApplicantApplicationNavigationService {
  private readonly stepsSignal = signal<ApplicantApplicationStep[]>([]);
  private readonly currentIndexSignal = signal(0);

  readonly steps = this.stepsSignal.asReadonly();
  readonly currentIndex = this.currentIndexSignal.asReadonly();

  registerSteps(steps: ApplicantApplicationStep[]): void {
    this.stepsSignal.set(steps);
    this.currentIndexSignal.update((current) => {
      if (!steps.length) {
        return 0;
      }
      return Math.min(current, steps.length);
    });
  }

  clear(): void {
    this.stepsSignal.set([]);
    this.currentIndexSignal.set(0);
  }

  setCurrentIndex(index: number): void {
    const steps = this.stepsSignal();
    if (!steps.length) {
      this.currentIndexSignal.set(0);
      return;
    }
    const clamped = Math.max(0, Math.min(index, steps.length));
    this.currentIndexSignal.set(clamped);
  }
}





