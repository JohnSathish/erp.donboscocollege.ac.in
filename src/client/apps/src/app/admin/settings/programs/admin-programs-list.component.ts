import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SettingsApiService, ProgramDto, ProgramsListResponse } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-programs-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-programs-list.component.html',
  styleUrls: ['./admin-programs-list.component.scss'],
})
export class AdminProgramsListComponent implements OnInit {
  private readonly api = inject(SettingsApiService);
  private readonly toast = inject(ToastService);

  programs = signal<ProgramDto[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = signal(20);
  isLoading = signal(false);
  searchTerm = signal('');
  isActiveFilter = signal<boolean | null>(null);

  ngOnInit(): void {
    this.loadPrograms();
  }

  async loadPrograms(): Promise<void> {
    this.isLoading.set(true);
    try {
      const response = await firstValueFrom(
        this.api.listPrograms({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          isActive: this.isActiveFilter() ?? undefined,
          searchTerm: this.searchTerm() || undefined,
        })
      );
      this.programs.set(response.programs);
      this.totalCount.set(response.totalCount);
    } catch (error) {
      console.error('Error loading programs:', error);
      this.toast.error('Failed to load programs');
    } finally {
      this.isLoading.set(false);
    }
  }

  onSearch(): void {
    this.currentPage.set(1);
    this.loadPrograms();
  }

  onFilterChange(): void {
    this.currentPage.set(1);
    this.loadPrograms();
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadPrograms();
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount() / this.pageSize());
  }

  async toggleStatus(programId: string): Promise<void> {
    try {
      await firstValueFrom(this.api.toggleProgramStatus(programId));
      this.toast.success('Program status updated');
      await this.loadPrograms();
    } catch (error) {
      console.error('Error toggling program status:', error);
      this.toast.error('Failed to update program status');
    }
  }

  async deleteProgram(programId: string): Promise<void> {
    if (!confirm('Are you sure you want to delete this program?')) {
      return;
    }

    try {
      await firstValueFrom(this.api.deleteProgram(programId));
      this.toast.success('Program deleted');
      await this.loadPrograms();
    } catch (error) {
      console.error('Error deleting program:', error);
      this.toast.error('Failed to delete program');
    }
  }
}

