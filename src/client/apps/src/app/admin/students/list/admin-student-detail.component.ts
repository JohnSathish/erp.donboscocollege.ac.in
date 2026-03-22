import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { StudentsApiService, StudentDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { LoggingService } from '../../../shared/logging.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-student-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-student-detail.component.html',
  styleUrls: ['./admin-student-detail.component.scss'],
})
export class AdminStudentDetailComponent implements OnInit {
  private readonly api = inject(StudentsApiService);
  protected readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly toast = inject(ToastService);
  private readonly logging = inject(LoggingService);

  protected readonly student = signal<StudentDto | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);

  async ngOnInit(): Promise<void> {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error.set('Student ID is required');
      return;
    }
    await this.loadStudent(id);
  }

  async loadStudent(id: string): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const student = await firstValueFrom(this.api.getStudent(id));
      this.student.set(student);
    } catch (error: any) {
      this.logging.error('Failed to load student', error);
      this.error.set(error.error?.message || 'Failed to load student details. Please try again.');
      this.toast.show('Failed to load student details', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  editStudent(): void {
    const student = this.student();
    if (student) {
      this.router.navigate(['/admin/students', student.id, 'edit']);
    }
  }

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




