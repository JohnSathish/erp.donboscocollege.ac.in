import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-admin-class-allocation',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: `<app-placeholder icon="🏫" title="Class & Section Allocation" description="This feature is under development and will be available soon."></app-placeholder>`,
})
export class AdminClassAllocationComponent {}
