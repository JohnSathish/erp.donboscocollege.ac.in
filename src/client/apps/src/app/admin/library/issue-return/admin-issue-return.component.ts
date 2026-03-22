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
import { LibraryApiService, BookIssueDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-issue-return',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-issue-return.component.html',
  styleUrls: ['./admin-issue-return.component.scss'],
})
export class AdminIssueReturnComponent implements OnInit {
  private readonly api = inject(LibraryApiService);
  protected readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly issues = signal<BookIssueDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly totalCount = signal(0);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(50);

  protected readonly statusFilterControl = this.fb.control<string>('');
  protected readonly searchControl = this.fb.control<string>('');

  protected readonly hasMore = computed(() => {
    const total = this.totalCount();
    const current = this.issues().length;
    return current < total;
  });

  async ngOnInit(): Promise<void> {
    await this.loadIssues();
  }

  async loadIssues(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const response = await firstValueFrom(
        this.api.listBookIssues({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          status: this.statusFilterControl.value || undefined,
        })
      );
      this.issues.set(response.issues);
      this.totalCount.set(response.totalCount);
      this.currentPage.set(response.page);
    } catch (error) {
      console.error('Failed to load book issues', error);
      this.error.set('Failed to load book issues. Please try again.');
      this.toast.show('Failed to load book issues', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onFilterChange(): Promise<void> {
    this.currentPage.set(1);
    await this.loadIssues();
  }

  async returnBook(issue: BookIssueDto): Promise<void> {
    if (issue.status !== 'Issued') {
      this.toast.show('This book is already returned or closed', 'error');
      return;
    }

    const returnDate = new Date().toISOString().split('T')[0];
    const fineAmount = this.calculateFine(issue);

    try {
      await firstValueFrom(
        this.api.returnBook({
          issueId: issue.id,
          returnDate: new Date().toISOString(),
          fineAmount: fineAmount > 0 ? fineAmount : null,
        })
      );
      this.toast.show('Book returned successfully', 'success');
      await this.loadIssues();
    } catch (error: any) {
      console.error('Failed to return book', error);
      this.toast.show(
        error?.error?.message || 'Failed to return book. Please try again.',
        'error'
      );
    }
  }

  protected calculateFine(issue: BookIssueDto): number {
    const dueDate = new Date(issue.dueDate);
    const today = new Date();
    const daysOverdue = Math.max(0, Math.floor((today.getTime() - dueDate.getTime()) / (1000 * 60 * 60 * 24)));
    
    // Fine calculation: ₹5 per day overdue (placeholder - can be configured)
    return daysOverdue * 5;
  }

  getStatusBadgeClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'issued':
        return 'badge--info';
      case 'returned':
        return 'badge--success';
      case 'overdue':
        return 'badge--danger';
      case 'lost':
      case 'damaged':
        return 'badge--warning';
      default:
        return 'badge--neutral';
    }
  }

  isOverdue(issue: BookIssueDto): boolean {
    if (issue.status !== 'Issued') return false;
    const dueDate = new Date(issue.dueDate);
    const today = new Date();
    return today > dueDate;
  }

  getDaysOverdue(issue: BookIssueDto): number {
    if (!this.isOverdue(issue)) return 0;
    const dueDate = new Date(issue.dueDate);
    const today = new Date();
    return Math.floor((today.getTime() - dueDate.getTime()) / (1000 * 60 * 60 * 24));
  }

  protected readonly Math = Math;
}
