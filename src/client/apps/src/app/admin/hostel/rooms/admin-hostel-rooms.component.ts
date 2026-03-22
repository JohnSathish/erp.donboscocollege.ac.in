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
import { HostelApiService, HostelRoomDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-hostel-rooms',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-hostel-rooms.component.html',
  styleUrls: ['./admin-hostel-rooms.component.scss'],
})
export class AdminHostelRoomsComponent implements OnInit {
  private readonly api = inject(HostelApiService);
  protected readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly rooms = signal<HostelRoomDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly totalCount = signal(0);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(50);

  protected readonly searchControl = this.fb.control<string>('');
  protected readonly blockFilterControl = this.fb.control<string>('');
  protected readonly roomTypeFilterControl = this.fb.control<string>('');
  protected readonly statusFilterControl = this.fb.control<string>('');

  protected readonly hasMore = computed(() => {
    const total = this.totalCount();
    const current = this.rooms().length;
    return current < total;
  });

  async ngOnInit(): Promise<void> {
    await this.loadRooms();
  }

  async loadRooms(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const response = await firstValueFrom(
        this.api.listHostelRooms({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          searchTerm: this.searchControl.value || undefined,
          blockName: this.blockFilterControl.value || undefined,
          roomType: this.roomTypeFilterControl.value || undefined,
          status: this.statusFilterControl.value || undefined,
        })
      );
      this.rooms.set(response.rooms);
      this.totalCount.set(response.totalCount);
      this.currentPage.set(response.page);
    } catch (error) {
      console.error('Failed to load hostel rooms', error);
      this.error.set('Failed to load hostel rooms. Please try again.');
      this.toast.show('Failed to load hostel rooms', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    this.currentPage.set(1);
    await this.loadRooms();
  }

  async onFilterChange(): Promise<void> {
    this.currentPage.set(1);
    await this.loadRooms();
  }

  getStatusBadgeClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'available':
        return 'badge--success';
      case 'partiallyoccupied':
        return 'badge--info';
      case 'full':
        return 'badge--neutral';
      case 'undermaintenance':
        return 'badge--warning';
      case 'reserved':
        return 'badge--secondary';
      default:
        return 'badge--neutral';
    }
  }

  getOccupancyClass(room: HostelRoomDto): string {
    if (room.availableBeds === 0) {
      return 'text-danger';
    }
    if (room.availableBeds < room.capacity * 0.3) {
      return 'text-warning';
    }
    return 'text-success';
  }

  protected readonly Math = Math;
}
