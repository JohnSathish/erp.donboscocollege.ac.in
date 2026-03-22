import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-admin-transport-form',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: `<app-placeholder icon="➕" title="Add Vehicle" description="Vehicle creation form is under development and will be available soon."></app-placeholder>`,
})
export class AdminTransportFormComponent {}




