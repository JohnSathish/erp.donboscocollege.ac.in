import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-admin-library-form',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: `<app-placeholder icon="➕" title="Add Book" description="Book creation form is under development and will be available soon."></app-placeholder>`,
})
export class AdminLibraryFormComponent {}




