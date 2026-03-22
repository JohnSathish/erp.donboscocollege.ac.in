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
import { LibraryApiService, BookDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-library-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-library-list.component.html',
  styleUrls: ['./admin-library-list.component.scss'],
})
export class AdminLibraryListComponent implements OnInit {
  private readonly api = inject(LibraryApiService);
  protected readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly books = signal<BookDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly totalCount = signal(0);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(50);

  protected readonly searchControl = this.fb.control<string>('');
  protected readonly categoryFilterControl = this.fb.control<string>('');
  protected readonly authorFilterControl = this.fb.control<string>('');
  protected readonly statusFilterControl = this.fb.control<string>('');

  protected readonly hasMore = computed(() => {
    const total = this.totalCount();
    const current = this.books().length;
    return current < total;
  });

  async ngOnInit(): Promise<void> {
    await this.loadBooks();
  }

  async loadBooks(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const response = await firstValueFrom(
        this.api.listBooks({
          page: this.currentPage(),
          pageSize: this.pageSize(),
          searchTerm: this.searchControl.value || undefined,
          category: this.categoryFilterControl.value || undefined,
          author: this.authorFilterControl.value || undefined,
          status: this.statusFilterControl.value || undefined,
        })
      );
      this.books.set(response.books);
      this.totalCount.set(response.totalCount);
      this.currentPage.set(response.page);
    } catch (error) {
      console.error('Failed to load books', error);
      this.error.set('Failed to load books. Please try again.');
      this.toast.show('Failed to load books', 'error');
    } finally {
      this.loading.set(false);
    }
  }

  async onSearch(): Promise<void> {
    this.currentPage.set(1);
    await this.loadBooks();
  }

  async onFilterChange(): Promise<void> {
    this.currentPage.set(1);
    await this.loadBooks();
  }

  issueBook(bookId: string): void {
    this.router.navigate(['/admin/library/issue', bookId]);
  }

  getStatusBadgeClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'available':
        return 'badge--success';
      case 'unavailable':
        return 'badge--danger';
      case 'lost':
      case 'damaged':
        return 'badge--warning';
      default:
        return 'badge--neutral';
    }
  }

  getAvailabilityClass(book: BookDto): string {
    if (book.availableCopies === 0) {
      return 'text-danger';
    }
    if (book.availableCopies < book.totalCopies * 0.2) {
      return 'text-warning';
    }
    return 'text-success';
  }

  protected readonly Math = Math;
}

