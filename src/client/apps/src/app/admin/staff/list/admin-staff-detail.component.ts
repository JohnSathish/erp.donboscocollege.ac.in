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
import { StaffApiService, StaffMemberDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { LoggingService } from '../../../shared/logging.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-staff-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-staff-detail.component.html',
  styleUrls: ['./admin-staff-detail.component.scss'],
})
export class AdminStaffDetailComponent implements OnInit {
  private readonly api = inject(StaffApiService);
  protected readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly toast = inject(ToastService);
  private readonly logger = inject(LoggingService);

  protected readonly staff = signal<StaffMemberDto | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);

  async ngOnInit(): Promise<void> {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error.set('Staff member ID is required');
      return;
    }
    await this.loadStaff(id);
  }

  async loadStaff(id: string): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const staffMember = await firstValueFrom(this.api.getStaffMember(id));
      this.staff.set(staffMember);
    } catch (error: any) {
      this.logger.error('Failed to load staff member', error);
      const errorMessage = error?.error?.message || error?.message || 'Failed to load staff member details. Please try again.';
      this.error.set(errorMessage);
      this.toast.show(errorMessage, 'error');
    } finally {
      this.loading.set(false);
    }
  }

  editStaff(): void {
    const staff = this.staff();
    if (staff) {
      this.router.navigate(['/admin/staff', staff.id, 'edit']);
    }
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
}
