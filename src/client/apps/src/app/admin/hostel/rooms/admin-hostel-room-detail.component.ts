import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-admin-hostel-room-detail',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: `<app-placeholder icon="👁️" title="Room Details" description="Hostel room detail view is under development and will be available soon."></app-placeholder>`,
})
export class AdminHostelRoomDetailComponent {}




