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
import { Router, ActivatedRoute } from '@angular/router';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
  FormArray,
  FormGroup,
} from '@angular/forms';
import {
  ExaminationsApiService,
  SettingsApiService,
  AssessmentDto,
  CourseDto,
  ProgramDto,
} from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { AuthService } from '../../../auth/auth.service';
import { LoggingService } from '../../../shared/logging.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-assessment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-assessment-form.component.html',
  styleUrls: ['./admin-assessment-form.component.scss'],
})
export class AdminAssessmentFormComponent implements OnInit {
  private readonly api = inject(ExaminationsApiService);
  private readonly settingsApi = inject(SettingsApiService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);
  private readonly authService = inject(AuthService);
  private readonly logger = inject(LoggingService);

  protected readonly loading = signal(false);
  protected readonly saving = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly isEditMode = signal(false);
  protected readonly assessmentId = signal<string | null>(null);

  protected readonly programs = signal<ProgramDto[]>([]);
  protected readonly courses = signal<CourseDto[]>([]);
  protected readonly assessmentTypes = [
    { value: 'MidTerm', label: 'Mid-Term Exam' },
    { value: 'Final', label: 'Final Exam' },
    { value: 'Quiz', label: 'Quiz' },
    { value: 'Assignment', label: 'Assignment' },
    { value: 'Project', label: 'Project' },
    { value: 'Practical', label: 'Practical' },
    { value: 'Internal', label: 'Internal Assessment' },
  ];

  protected readonly assessmentForm = this.fb.group({
    courseId: ['', [Validators.required]],
    academicTermId: ['', [Validators.required]],
    name: ['', [Validators.required, Validators.maxLength(200)]],
    code: ['', [Validators.required, Validators.maxLength(50)]],
    type: ['', [Validators.required]],
    maxMarks: [100, [Validators.required, Validators.min(1), Validators.max(1000)]],
    passingMarks: [40, [Validators.required, Validators.min(0)]],
    totalWeightage: [100, [Validators.required, Validators.min(0), Validators.max(100)]],
    classSectionId: [''],
    scheduledDate: [''],
    duration: [''],
    instructions: [''],
    components: this.fb.array<FormGroup>([]),
  });

  protected readonly totalComponentWeightage = computed(() => {
    const components = this.componentsArray.controls;
    return components.reduce((sum, control) => {
      const weightage = control.get('weightage')?.value || 0;
      return sum + (typeof weightage === 'number' ? weightage : parseFloat(weightage) || 0);
    }, 0);
  });

  async ngOnInit(): Promise<void> {
    await this.loadPrograms();
    
    // Check if editing
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.assessmentId.set(id);
      this.isEditMode.set(true);
      await this.loadAssessment(id);
    } else {
      // Add at least one component by default
      this.addComponent();
    }
  }

  async loadPrograms(): Promise<void> {
    try {
      const response = await firstValueFrom(
        this.settingsApi.listPrograms({ page: 1, pageSize: 100, isActive: true })
      );
      this.programs.set(response.programs);
    } catch (error) {
      this.logger.error('Failed to load programs', error);
    }
  }

  async loadCourses(programId?: string): Promise<void> {
    try {
      const response = await firstValueFrom(
        this.settingsApi.listCourses({
          page: 1,
          pageSize: 100,
          isActive: true,
          programId: programId || undefined,
        })
      );
      this.courses.set(response.courses);
    } catch (error) {
      this.logger.error('Failed to load courses', error);
    }
  }

  async loadAssessment(id: string): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const assessment = await firstValueFrom(this.api.getAssessment(id));
      
      // Patch form with assessment data
      this.assessmentForm.patchValue({
        courseId: assessment.courseId,
        academicTermId: assessment.academicTermId,
        name: assessment.name,
        code: assessment.code,
        type: assessment.type,
        maxMarks: assessment.maxMarks,
        passingMarks: assessment.passingMarks,
        totalWeightage: assessment.totalWeightage,
        classSectionId: assessment.classSectionId || '',
        scheduledDate: assessment.scheduledDate
          ? new Date(assessment.scheduledDate).toISOString().split('T')[0]
          : '',
        duration: assessment.duration || '',
        instructions: assessment.instructions || '',
      });

      // Load courses for the selected program
      await this.loadCourses();

      // Load components
      this.clearComponents();
      if (assessment.components && assessment.components.length > 0) {
        assessment.components.forEach((component) => {
          this.addComponent(component);
        });
      } else {
        this.addComponent();
      }
    } catch (error: any) {
      this.logger.error('Failed to load assessment', error);
      this.error.set('Failed to load assessment. Please try again.');
      this.toast.show('Failed to load assessment', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  onProgramChange(): void {
    // When program changes, reload courses
    // For now, we'll load all courses. In future, filter by program
    this.loadCourses();
  }

  get componentsArray(): FormArray {
    return this.assessmentForm.get('components') as FormArray;
  }

  getComponentFormGroup(index: number): FormGroup {
    return this.componentsArray.at(index) as FormGroup;
  }

  addComponent(component?: any): void {
    const group = this.fb.group({
      name: [component?.name || '', [Validators.required, Validators.maxLength(200)]],
      code: [component?.code || ''],
      maxMarks: [
        component?.maxMarks ?? 0,
        [Validators.required, Validators.min(1), Validators.max(1000)],
      ],
      passingMarks: [
        component?.passingMarks ?? 0,
        [Validators.required, Validators.min(0)],
      ],
      weightage: [
        component?.weightage ?? 0,
        [Validators.required, Validators.min(0), Validators.max(100)],
      ],
      displayOrder: [component?.displayOrder ?? this.componentsArray.length + 1, [Validators.required]],
      instructions: [component?.instructions || ''],
    });
    this.componentsArray.push(group);
  }

  removeComponent(index: number): void {
    if (this.componentsArray.length > 1) {
      this.componentsArray.removeAt(index);
      // Update display orders
      this.updateDisplayOrders();
    } else {
      this.toast.show('At least one component is required', 'error');
    }
  }

  private updateDisplayOrders(): void {
    this.componentsArray.controls.forEach((control, index) => {
      control.patchValue({ displayOrder: index + 1 }, { emitEvent: false });
    });
  }

  private clearComponents(): void {
    while (this.componentsArray.length !== 0) {
      this.componentsArray.removeAt(0);
    }
  }

  async saveAssessment(): Promise<void> {
    if (this.assessmentForm.invalid) {
      this.assessmentForm.markAllAsTouched();
      this.toast.show('Please fill in all required fields correctly', 'error');
      return;
    }

    // Validate component weightage matches total weightage
    const totalWeightage = this.assessmentForm.get('totalWeightage')?.value || 0;
    const componentWeightage = this.totalComponentWeightage();
    
    if (this.abs(componentWeightage - totalWeightage) > 0.01) {
      this.toast.show(
        `Component weightage total (${componentWeightage}%) does not match assessment weightage (${totalWeightage}%). Please adjust.`,
        'error'
      );
      // Still allow saving, but warn
    }

    this.saving.set(true);
    try {
      const formValue = this.assessmentForm.getRawValue();
      
      const payload = {
        courseId: formValue.courseId!,
        academicTermId: formValue.academicTermId!,
        name: formValue.name!,
        code: formValue.code!,
        type: formValue.type!,
        maxMarks: formValue.maxMarks!,
        passingMarks: formValue.passingMarks!,
        totalWeightage: formValue.totalWeightage!,
        classSectionId: formValue.classSectionId || null,
        scheduledDate: formValue.scheduledDate ? new Date(formValue.scheduledDate).toISOString() : null,
        duration: formValue.duration || null,
        instructions: formValue.instructions || null,
        components: this.componentsArray.controls.map((control) => ({
          name: control.get('name')!.value,
          maxMarks: control.get('maxMarks')!.value,
          passingMarks: control.get('passingMarks')!.value,
          weightage: control.get('weightage')!.value,
          displayOrder: control.get('displayOrder')!.value,
          code: control.get('code')?.value || null,
          instructions: control.get('instructions')?.value || null,
        })),
        createdBy: this.getCurrentUser(),
      };

      await firstValueFrom(this.api.createAssessment(payload));

      this.toast.show(
        `Assessment ${this.isEditMode() ? 'updated' : 'created'} successfully`,
        'success'
      );

      // Navigate back to list
      this.router.navigate(['/admin/examination/assessments']);
    } catch (error: any) {
      this.logger.error('Failed to save assessment', error);
      this.error.set(error.error?.message || 'Failed to save assessment. Please try again.');
      this.toast.show(
        error.error?.message || 'Failed to save assessment. Please try again.',
        'error'
      );
    } finally {
      this.saving.set(false);
    }
  }

  cancel(): void {
    this.router.navigate(['/admin/examination/assessments']);
  }

  private getCurrentUser(): string {
    const profile = this.authService.profile;
    return profile?.fullName || profile?.email || profile?.uniqueId || 'System';
  }

  protected abs(value: number): number {
    return Math.abs(value);
  }
}
