import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AdmissionsAdminApiService, AdmittedStudentRowDto } from '@client/shared/data';
import { firstValueFrom } from 'rxjs';
import { ToastService } from '../../../shared/toast.service';

@Component({
  selector: 'app-admin-admitted-students',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-admitted-students.component.html',
  styleUrls: ['./admin-admitted-students.component.scss'],
})
export class AdminAdmittedStudentsComponent implements OnInit {
  private readonly api = inject(AdmissionsAdminApiService);
  private readonly toast = inject(ToastService);
  private readonly fb = inject(FormBuilder);

  protected readonly searchControl = this.fb.control('');
  protected readonly rows = signal<AdmittedStudentRowDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly totalCount = signal(0);
  protected readonly page = signal(1);
  protected readonly pageSize = signal(25);

  async ngOnInit(): Promise<void> {
    await this.load();
  }

  async load(): Promise<void> {
    this.loading.set(true);
    try {
      const res = await firstValueFrom(
        this.api.listAdmittedStudents({
          page: this.page(),
          pageSize: this.pageSize(),
          searchTerm: this.searchControl.value || undefined,
        })
      );
      const list = (res as { items?: AdmittedStudentRowDto[] }).items ?? [];
      this.rows.set(list);
      this.totalCount.set(res.totalCount);
    } catch {
      this.toast.error('Unable to load admitted students.');
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    this.page.set(1);
    await this.load();
  }

  async nextPage(): Promise<void> {
    const max = Math.max(1, Math.ceil(this.totalCount() / this.pageSize()));
    if (this.page() >= max) {
      return;
    }
    this.page.update((p) => p + 1);
    await this.load();
  }

  async prevPage(): Promise<void> {
    if (this.page() <= 1) {
      return;
    }
    this.page.update((p) => p - 1);
    await this.load();
  }

  paymentBadgeClass(row: AdmittedStudentRowDto): string {
    if (row.status === 'Enrolled') {
      return 'badge-paid';
    }
    if (row.isPaymentCompleted) {
      return 'badge-paid';
    }
    return 'badge-pending';
  }

  paymentLabel(row: AdmittedStudentRowDto): string {
    if (row.status === 'Enrolled') {
      return 'Complete';
    }
    return row.isPaymentCompleted ? 'Paid' : 'Pending';
  }
}
