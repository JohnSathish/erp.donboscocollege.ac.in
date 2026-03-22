import { Component, Input, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-kpi-card',
  standalone: true,
  imports: [CommonModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <div class="card shadow-none border border-gray-200 dark:border-neutral-600 dark:bg-neutral-700 rounded-lg h-full bg-gradient-to-r {{ gradientClass }}">
      <div class="card-body p-5">
        <div class="flex flex-wrap items-center justify-between gap-3">
          <div>
            <p class="font-medium text-neutral-900 dark:text-white mb-1">{{ label }}</p>
            <h6 class="mb-0 dark:text-white text-2xl font-semibold">{{ value | number }}</h6>
          </div>
          <div class="w-[50px] h-[50px] {{ iconBgClass }} rounded-full flex justify-center items-center">
            <iconify-icon [icon]="icon" class="text-white text-2xl mb-0"></iconify-icon>
          </div>
        </div>
        <p class="font-medium text-sm text-neutral-600 dark:text-white mt-3 mb-0 flex items-center gap-2" *ngIf="trend">
          <span class="inline-flex items-center gap-1 {{ trendColorClass }}">
            <iconify-icon [icon]="trendIcon" class="text-xs"></iconify-icon> {{ trendValue }}
          </span> 
          {{ trendLabel }}
        </p>
      </div>
    </div>
  `,
  styles: []
})
export class KpiCardComponent {
  @Input() label: string = '';
  @Input() value: number = 0;
  @Input() icon: string = '';
  @Input() iconBgClass: string = 'bg-primary-600';
  @Input() gradientClass: string = 'from-primary-600/10 to-bg-white';
  @Input() trend?: {
    value: string;
    label: string;
    isPositive: boolean;
  };
  
  get trendColorClass(): string {
    return this.trend?.isPositive 
      ? 'text-success-600 dark:text-success-400' 
      : 'text-danger-600 dark:text-danger-400';
  }
  
  get trendIcon(): string {
    return this.trend?.isPositive ? 'bxs:up-arrow' : 'bxs:down-arrow';
  }
  
  get trendValue(): string {
    return this.trend?.value || '';
  }
  
  get trendLabel(): string {
    return this.trend?.label || '';
  }
}

