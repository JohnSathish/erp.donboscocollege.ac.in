import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ADMISSION_WORKFLOW_STAGES } from '../admission-workflow';

@Component({
  selector: 'app-admission-workflow-guide',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './admission-workflow-guide.component.html',
  styleUrl: './admission-workflow-guide.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdmissionWorkflowGuideComponent {
  protected readonly stages = ADMISSION_WORKFLOW_STAGES;
}
