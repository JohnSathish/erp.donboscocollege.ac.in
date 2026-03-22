import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SettingsApiService, CourseDto, CoursesListResponse } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-courses-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-courses-list.component.html',
  styleUrls: ['./admin-courses-list.component.scss'],
})
export class AdminCoursesListComponent implements OnInit {
  private readonly api = inject(SettingsApiService);
  private readonly toast = inject(ToastService);

  courses = signal<CourseDto[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = signal(20);
  isLoading = signal(false);
  searchTerm = signal('');
  isActiveFilter = signal<boolean | null>(null);
  programIdFilter = signal<string | null>(null);
  programs = signal<Array<{ id: string; name: string }>>([]);

  ngOnInit(): void {
    this.loadCourses();
  }

  async loadCourses(): Promise<void> {
    this.isLoading.set(true);
    try {
      const response = await firstValueFrom(
        this.api.listCourses({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          isActive: this.isActiveFilter() ?? undefined,
          programId: this.programIdFilter() ?? undefined,
          searchTerm: this.searchTerm() || undefined,
        })
      );
      this.courses.set(response.courses);
      this.totalCount.set(response.totalCount);
    } catch (error) {
      console.error('Error loading courses:', error);
      this.toast.error('Failed to load courses');
    } finally {
      this.isLoading.set(false);
    }
  }

  onSearch(): void {
    this.currentPage.set(1);
    this.loadCourses();
  }

  onFilterChange(): void {
    this.currentPage.set(1);
    this.loadCourses();
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadCourses();
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount() / this.pageSize());
  }

  async toggleStatus(courseId: string): Promise<void> {
    try {
      await firstValueFrom(this.api.toggleCourseStatus(courseId));
      this.toast.success('Course status updated');
      await this.loadCourses();
    } catch (error) {
      console.error('Error toggling course status:', error);
      this.toast.error('Failed to update course status');
    }
  }

  async deleteCourse(courseId: string): Promise<void> {
    if (!confirm('Are you sure you want to delete this course?')) {
      return;
    }

    try {
      await firstValueFrom(this.api.deleteCourse(courseId));
      this.toast.success('Course deleted');
      await this.loadCourses();
    } catch (error) {
      console.error('Error deleting course:', error);
      this.toast.error('Failed to delete course');
    }
  }
}

