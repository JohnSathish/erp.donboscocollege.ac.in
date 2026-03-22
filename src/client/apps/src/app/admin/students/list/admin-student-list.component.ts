import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { StudentsApiService, StudentDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-student-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-student-list.component.html',
  styleUrls: ['./admin-student-list.component.scss'],
})
export class AdminStudentListComponent implements OnInit {
  private readonly api = inject(StudentsApiService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly students = signal<StudentDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly totalCount = signal(0);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(50);

  protected readonly searchControl = this.fb.control<string>('');
  protected readonly statusFilterControl = this.fb.control<string>('');
  protected readonly academicYearFilterControl = this.fb.control<string>('');
  protected readonly programFilterControl = this.fb.control<string>('');

  protected readonly hasMore = computed(() => {
    const total = this.totalCount();
    const current = this.students().length;
    return current < total;
  });

  async ngOnInit(): Promise<void> {
    await this.loadStudents();
  }

  async loadStudents(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const statusValue = this.statusFilterControl.value;
      const isActive =
        statusValue === 'Active'
          ? true
          : statusValue === 'Inactive'
          ? false
          : undefined;

      const response = await firstValueFrom(
        this.api.listStudents({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          searchTerm: this.searchControl.value || undefined,
          isActive: isActive,
          academicYear: this.academicYearFilterControl.value || undefined,
          programId: this.programFilterControl.value || undefined,
        })
      );
      this.students.set(response.students);
      this.totalCount.set(response.totalCount);
      this.currentPage.set(response.page);
    } catch (error: any) {
      console.error('Failed to load students', error);
      this.error.set('Failed to load students. Please try again.');
      this.toast.show('Failed to load students', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    this.currentPage.set(1);
    await this.loadStudents();
  }

  async onFilterChange(): Promise<void> {
    this.currentPage.set(1);
    await this.loadStudents();
  }

  viewStudent(studentId: string): void {
    this.router.navigate(['/admin/students', studentId]);
  }

  editStudent(studentId: string): void {
    this.router.navigate(['/admin/students', studentId, 'edit']);
  }

  protected readonly Math = Math;

  getStatusBadgeClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
        return 'badge--success';
      case 'inactive':
        return 'badge--warning';
      case 'transferred':
        return 'badge--info';
      case 'graduated':
        return 'badge--primary';
      case 'withdrawn':
      case 'suspended':
        return 'badge--danger';
      default:
        return 'badge--neutral';
    }
  }
}
