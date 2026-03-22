import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';
import {
  BookDto,
  BooksListResponse,
  CreateBookPayload,
  UpdateBookPayload,
  IssueBookPayload,
  ReturnBookPayload,
  BookIssuesListResponse,
} from '../dtos/library.dto';

@Injectable({ providedIn: 'root' })
export class LibraryApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  listBooks(params?: {
    page?: number;
    pageSize?: number;
    category?: string;
    author?: string;
    status?: string;
    searchTerm?: string;
  }): Observable<BooksListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) {
        httpParams = httpParams.set('page', params.page.toString());
      }
      if (params.pageSize) {
        httpParams = httpParams.set('pageSize', params.pageSize.toString());
      }
      if (params.category) {
        httpParams = httpParams.set('category', params.category);
      }
      if (params.author) {
        httpParams = httpParams.set('author', params.author);
      }
      if (params.status) {
        httpParams = httpParams.set('status', params.status);
      }
      if (params.searchTerm) {
        httpParams = httpParams.set('searchTerm', params.searchTerm);
      }
    }
    return this.http.get<BooksListResponse>(
      `${this.apiBaseUrl}/library/books`,
      { params: httpParams }
    );
  }

  getBook(bookId: string): Observable<BookDto> {
    return this.http.get<BookDto>(
      `${this.apiBaseUrl}/library/books/${bookId}`
    );
  }

  createBook(payload: CreateBookPayload): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/library/books`,
      payload
    );
  }

  updateBook(bookId: string, payload: UpdateBookPayload): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/library/books/${bookId}`,
      payload
    );
  }

  issueBook(payload: IssueBookPayload): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/library/books/issue`,
      payload
    );
  }

  returnBook(payload: ReturnBookPayload): Observable<void> {
    return this.http.post<void>(
      `${this.apiBaseUrl}/library/books/return`,
      payload
    );
  }

  listBookIssues(params?: {
    page?: number;
    pageSize?: number;
    bookId?: string;
    studentId?: string;
    staffId?: string;
    status?: string;
  }): Observable<BookIssuesListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) {
        httpParams = httpParams.set('page', params.page.toString());
      }
      if (params.pageSize) {
        httpParams = httpParams.set('pageSize', params.pageSize.toString());
      }
      if (params.bookId) {
        httpParams = httpParams.set('bookId', params.bookId);
      }
      if (params.studentId) {
        httpParams = httpParams.set('studentId', params.studentId);
      }
      if (params.staffId) {
        httpParams = httpParams.set('staffId', params.staffId);
      }
      if (params.status) {
        httpParams = httpParams.set('status', params.status);
      }
    }
    return this.http.get<BookIssuesListResponse>(
      `${this.apiBaseUrl}/library/issues`,
      { params: httpParams }
    );
  }
}

