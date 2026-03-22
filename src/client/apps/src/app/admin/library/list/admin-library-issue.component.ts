import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-admin-library-issue',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: `<app-placeholder icon="📚" title="Issue Book" description="Book issue form is under development and will be available soon."></app-placeholder>`,
})
export class AdminLibraryIssueComponent {}




