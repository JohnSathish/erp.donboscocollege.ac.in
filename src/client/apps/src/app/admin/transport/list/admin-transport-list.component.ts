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
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { TransportApiService, VehicleDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-transport-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-transport-list.component.html',
  styleUrls: ['./admin-transport-list.component.scss'],
})
export class AdminTransportListComponent implements OnInit {
  private readonly api = inject(TransportApiService);
  protected readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly vehicles = signal<VehicleDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly totalCount = signal(0);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(50);

  protected readonly searchControl = this.fb.control<string>('');
  protected readonly vehicleTypeFilterControl = this.fb.control<string>('');
  protected readonly statusFilterControl = this.fb.control<string>('');

  protected readonly hasMore = computed(() => {
    const total = this.totalCount();
    const current = this.vehicles().length;
    return current < total;
  });

  async ngOnInit(): Promise<void> {
    await this.loadVehicles();
  }

  async loadVehicles(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const response = await firstValueFrom(
        this.api.listVehicles({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          searchTerm: this.searchControl.value || undefined,
          vehicleType: this.vehicleTypeFilterControl.value || undefined,
          status: this.statusFilterControl.value || undefined,
        })
      );
      this.vehicles.set(response.vehicles);
      this.totalCount.set(response.totalCount);
      this.currentPage.set(response.page);
    } catch (error) {
      console.error('Failed to load vehicles', error);
      this.error.set('Failed to load vehicles. Please try again.');
      this.toast.show('Failed to load vehicles', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    this.currentPage.set(1);
    await this.loadVehicles();
  }

  async onFilterChange(): Promise<void> {
    this.currentPage.set(1);
    await this.loadVehicles();
  }

  getStatusBadgeClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
        return 'badge--success';
      case 'inactive':
        return 'badge--neutral';
      case 'undermaintenance':
        return 'badge--warning';
      case 'retired':
        return 'badge--danger';
      default:
        return 'badge--neutral';
    }
  }

  isRegistrationExpiring(vehicle: VehicleDto): boolean {
    if (!vehicle.registrationExpiryDate) return false;
    const expiryDate = new Date(vehicle.registrationExpiryDate);
    const today = new Date();
    const daysUntilExpiry = Math.floor((expiryDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    return daysUntilExpiry <= 30 && daysUntilExpiry >= 0;
  }

  isRegistrationExpired(vehicle: VehicleDto): boolean {
    if (!vehicle.registrationExpiryDate) return false;
    const expiryDate = new Date(vehicle.registrationExpiryDate);
    const today = new Date();
    return today > expiryDate;
  }

  protected readonly Math = Math;
}




