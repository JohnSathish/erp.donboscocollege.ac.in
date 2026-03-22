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
import { StaffApiService, StaffMemberDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-staff-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-staff-list.component.html',
  styleUrls: ['./admin-staff-list.component.scss'],
})
export class AdminStaffListComponent implements OnInit {
  private readonly api = inject(StaffApiService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly staff = signal<StaffMemberDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly totalCount = signal(0);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(50);

  protected readonly searchControl = this.fb.control<string>('');
  protected readonly departmentFilterControl = this.fb.control<string>('');
  protected readonly employeeTypeFilterControl = this.fb.control<string>('');
  protected readonly statusFilterControl = this.fb.control<string>('');

  protected readonly hasMore = computed(() => {
    const total = this.totalCount();
    const current = this.staff().length;
    return current < total;
  });

  async ngOnInit(): Promise<void> {
    await this.loadStaff();
  }

  async loadStaff(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const response = await firstValueFrom(
        this.api.listStaffMembers({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          searchTerm: this.searchControl.value || undefined,
          department: this.departmentFilterControl.value || undefined,
          employeeType: this.employeeTypeFilterControl.value || undefined,
          status: this.statusFilterControl.value || undefined,
        })
      );
      this.staff.set(response.staff);
      this.totalCount.set(response.totalCount);
      this.currentPage.set(response.page);
    } catch (error) {
      console.error('Failed to load staff', error);
      this.error.set('Failed to load staff members. Please try again.');
      this.toast.show('Failed to load staff members', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    this.currentPage.set(1);
    await this.loadStaff();
  }

  async onFilterChange(): Promise<void> {
    this.currentPage.set(1);
    await this.loadStaff();
  }

  viewStaff(staffId: string): void {
    this.router.navigate(['/admin/staff', staffId]);
  }

  createStaff(): void {
    this.router.navigate(['/admin/staff/new']);
  }

  getStatusBadgeClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
        return 'badge--success';
      case 'onleave':
        return 'badge--warning';
      case 'suspended':
        return 'badge--danger';
      case 'resigned':
      case 'terminated':
        return 'badge--neutral';
      default:
        return 'badge--neutral';
    }
  }

  getEmployeeTypeBadgeClass(type: string | null | undefined): string {
    if (!type) return 'badge--neutral';
    switch (type.toLowerCase()) {
      case 'teaching':
        return 'badge--info';
      case 'non-teaching':
        return 'badge--secondary';
      case 'administrative':
        return 'badge--primary';
      default:
        return 'badge--neutral';
    }
  }

  protected readonly Math = Math;
}
