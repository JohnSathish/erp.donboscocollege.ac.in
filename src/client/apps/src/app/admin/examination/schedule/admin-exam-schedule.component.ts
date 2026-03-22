import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-admin-exam-schedule',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: `<app-placeholder icon="📅" title="Exam Schedule" description="This feature is under development and will be available soon."></app-placeholder>`,
})
export class AdminExamScheduleComponent {}
