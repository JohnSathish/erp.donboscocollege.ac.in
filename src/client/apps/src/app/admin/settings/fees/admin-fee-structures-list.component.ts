import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SettingsApiService, FeeStructureDto, FeeStructuresListResponse } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-fee-structures-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, CurrencyPipe],
  templateUrl: './admin-fee-structures-list.component.html',
  styleUrls: ['./admin-fee-structures-list.component.scss'],
})
export class AdminFeeStructuresListComponent implements OnInit {
  private readonly api = inject(SettingsApiService);
  private readonly toast = inject(ToastService);

  feeStructures = signal<FeeStructureDto[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = signal(20);
  isLoading = signal(false);
  searchTerm = signal('');
  isActiveFilter = signal<boolean | null>(null);
  academicYearFilter = signal<string>('');

  ngOnInit(): void {
    this.loadFeeStructures();
  }

  async loadFeeStructures(): Promise<void> {
    this.isLoading.set(true);
    try {
      const response = await firstValueFrom(
        this.api.listFeeStructures({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          isActive: this.isActiveFilter() ?? undefined,
          academicYear: this.academicYearFilter() || undefined,
          searchTerm: this.searchTerm() || undefined,
        })
      );
      this.feeStructures.set(response.feeStructures);
      this.totalCount.set(response.totalCount);
    } catch (error) {
      console.error('Error loading fee structures:', error);
      this.toast.error('Failed to load fee structures');
    } finally {
      this.isLoading.set(false);
    }
  }

  onSearch(): void {
    this.currentPage.set(1);
    this.loadFeeStructures();
  }

  onFilterChange(): void {
    this.currentPage.set(1);
    this.loadFeeStructures();
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadFeeStructures();
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount() / this.pageSize());
  }

  async toggleStatus(feeStructureId: string): Promise<void> {
    try {
      await firstValueFrom(this.api.toggleFeeStructureStatus(feeStructureId));
      this.toast.success('Fee structure status updated');
      await this.loadFeeStructures();
    } catch (error) {
      console.error('Error toggling fee structure status:', error);
      this.toast.error('Failed to update fee structure status');
    }
  }

  async deleteFeeStructure(feeStructureId: string): Promise<void> {
    if (!confirm('Are you sure you want to delete this fee structure?')) {
      return;
    }

    try {
      await firstValueFrom(this.api.deleteFeeStructure(feeStructureId));
      this.toast.success('Fee structure deleted');
      await this.loadFeeStructures();
    } catch (error) {
      console.error('Error deleting fee structure:', error);
      this.toast.error('Failed to delete fee structure');
    }
  }
}

