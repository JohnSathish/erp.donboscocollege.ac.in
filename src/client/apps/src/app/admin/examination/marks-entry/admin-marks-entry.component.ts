import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  FormsModule,
  Validators,
  FormArray,
  FormGroup,
} from '@angular/forms';
import {
  ExaminationsApiService,
  AssessmentSummaryDto,
  AssessmentDto,
  AssessmentComponentDto,
  MarkEntryDto,
} from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { AuthService } from '../../../auth/auth.service';
import { LoggingService } from '../../../shared/logging.service';
import { firstValueFrom } from 'rxjs';

interface StudentMarkForm {
  studentId: string;
  studentName: string;
  studentRollNumber?: string;
  marksObtained: number | null;
  isAbsent: boolean;
  isExempted: boolean;
  remarks?: string;
  existingMark?: MarkEntryDto;
}

@Component({
  selector: 'app-admin-marks-entry',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-marks-entry.component.html',
  styleUrls: ['./admin-marks-entry.component.scss'],
})
export class AdminMarksEntryComponent implements OnInit {
  private readonly api = inject(ExaminationsApiService);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);
  private readonly authService = inject(AuthService);
  private readonly logger = inject(LoggingService);

  protected readonly assessments = signal<AssessmentSummaryDto[]>([]);
  protected readonly selectedAssessment = signal<AssessmentDto | null>(null);
  protected readonly selectedComponent = signal<AssessmentComponentDto | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly saving = signal(false);

  protected readonly assessmentForm = this.fb.group({
    assessmentId: ['', [Validators.required]],
    componentId: ['', [Validators.required]],
  });

  protected readonly marksForm = this.fb.group({
    students: this.fb.array<FormGroup>([]),
  });

  protected readonly maxMarks = computed(() => {
    const component = this.selectedComponent();
    return component?.maxMarks ?? 0;
  });

  protected readonly passingMarks = computed(() => {
    const component = this.selectedComponent();
    return component?.passingMarks ?? 0;
  });

  async ngOnInit(): Promise<void> {
    await this.loadAssessments();
  }

  async loadAssessments(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const assessments = await firstValueFrom(this.api.listAssessments());
      this.assessments.set(assessments);
    } catch (error) {
      this.logger.error('Failed to load assessments', error);
      this.error.set('Failed to load assessments. Please try again.');
      this.toast.show('Failed to load assessments', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onAssessmentChange(): Promise<void> {
    const assessmentId = this.assessmentForm.get('assessmentId')?.value;
    if (!assessmentId) {
      this.selectedAssessment.set(null);
      this.selectedComponent.set(null);
      this.clearStudents();
      return;
    }

    this.loading.set(true);
    try {
      const assessment = await firstValueFrom(this.api.getAssessment(assessmentId));
      this.selectedAssessment.set(assessment);
      
      // Reset component selection
      this.assessmentForm.patchValue({ componentId: '' });
      this.selectedComponent.set(null);
      this.clearStudents();
    } catch (error) {
      this.logger.error('Failed to load assessment', error);
      this.toast.show('Failed to load assessment details', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  onComponentChange(): void {
    const componentId = this.assessmentForm.get('componentId')?.value;
    const assessment = this.selectedAssessment();
    
    if (!componentId || !assessment) {
      this.selectedComponent.set(null);
      this.clearStudents();
      return;
    }

    const component = assessment.components.find(c => c.id === componentId);
    this.selectedComponent.set(component || null);
    
    // For now, we'll use a placeholder student list
    // In a real implementation, you'd fetch students from the class section
    this.initializeStudentMarks();
  }

  private initializeStudentMarks(): void {
    // TODO: Fetch actual students from the class section
    // For now, create a form that allows manual entry
    // This would typically come from: ClassSection -> CourseEnrollments -> Students
    this.clearStudents();
    
    // Placeholder: Add 5 empty student rows for demonstration
    // In production, fetch from API based on classSectionId
    for (let i = 0; i < 5; i++) {
      this.addStudentRow();
    }
  }

  private addStudentRow(student?: StudentMarkForm): void {
    const studentsArray = this.marksForm.get('students') as FormArray;
    const group = this.fb.group({
      studentId: [student?.studentId || '', [Validators.required]],
      studentName: [student?.studentName || '', [Validators.required]],
      studentRollNumber: [student?.studentRollNumber || ''],
      marksObtained: [
        student?.marksObtained ?? null,
        [Validators.required, Validators.min(0), Validators.max(this.maxMarks())],
      ],
      isAbsent: [student?.isAbsent || false],
      isExempted: [student?.isExempted || false],
      remarks: [student?.remarks || ''],
    });
    studentsArray.push(group);
  }

  private clearStudents(): void {
    const studentsArray = this.marksForm.get('students') as FormArray;
    while (studentsArray.length !== 0) {
      studentsArray.removeAt(0);
    }
  }

  get studentsArray(): FormArray {
    return this.marksForm.get('students') as FormArray;
  }

  getStudentFormGroup(index: number): FormGroup {
    return this.studentsArray.at(index) as FormGroup;
  }

  calculatePercentage(marks: number | null, maxMarks: number): number | null {
    if (marks === null || marks < 0 || maxMarks <= 0) {
      return null;
    }
    return Math.round((marks / maxMarks) * 100 * 100) / 100; // Round to 2 decimal places
  }

  isPassing(marks: number | null): boolean {
    if (marks === null) return false;
    return marks >= this.passingMarks();
  }

  async saveMarks(): Promise<void> {
    if (this.marksForm.invalid) {
      this.marksForm.markAllAsTouched();
      this.toast.show('Please fill in all required fields correctly', 'error');
      return;
    }

    const componentId = this.assessmentForm.get('componentId')?.value;
    if (!componentId) {
      this.toast.show('Please select an assessment component', 'error');
      return;
    }

    this.saving.set(true);
    try {
      const studentsArray = this.marksForm.get('students') as FormArray;
      const studentMarks = studentsArray.controls
        .map((control) => {
          const value = control.value;
          return {
            studentId: value.studentId,
            marksObtained: value.marksObtained ?? 0,
            isAbsent: value.isAbsent || false,
            isExempted: value.isExempted || false,
            remarks: value.remarks || null,
          };
        })
        .filter((sm) => sm.studentId); // Filter out empty rows

      if (studentMarks.length === 0) {
        this.toast.show('Please enter at least one student mark', 'error');
        return;
      }

      const result = await firstValueFrom(
        this.api.bulkEnterMarks({
          assessmentComponentId: componentId,
          studentMarks,
          enteredBy: this.getCurrentUser(),
        })
      );

      this.toast.show(
        `Successfully entered marks for ${result.count} students`,
        'success'
      );
      
      // Reload to show updated marks
      await this.onComponentChange();
    } catch (error: any) {
      this.logger.error('Failed to save marks', error);
      this.toast.show(
        error.error?.message || 'Failed to save marks. Please try again.',
        'error'
      );
    } finally {
      this.saving.set(false);
    }
  }

  addMoreStudents(): void {
    this.addStudentRow();
  }

  removeStudent(index: number): void {
    this.studentsArray.removeAt(index);
  }

  private getCurrentUser(): string {
    const profile = this.authService.profile;
    return profile?.fullName || profile?.email || profile?.uniqueId || 'System';
  }
}
