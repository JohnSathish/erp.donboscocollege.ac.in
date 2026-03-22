import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-admin-vehicle-details',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: `<app-placeholder icon="🚗" title="Vehicle Details" description="This feature is under development and will be available soon."></app-placeholder>`,
})
export class AdminVehicleDetailsComponent {}
