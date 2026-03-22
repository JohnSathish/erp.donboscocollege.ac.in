import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-admin-fee-concession',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: `<app-placeholder icon="🎫" title="Concession / Waiver" description="This feature is under development and will be available soon."></app-placeholder>`,
})
export class AdminFeeConcessionComponent {}
