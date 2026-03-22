// Library Module DTOs

export interface BookDto {
  id: string;
  isbn: string;
  title: string;
  author: string;
  publisher?: string | null;
  publicationYear?: number | null;
  category?: string | null;
  language?: string | null;
  totalCopies: number;
  availableCopies: number;
  price?: number | null;
  location?: string | null;
  description?: string | null;
  status: string;
  createdOnUtc: string;
  createdBy?: string | null;
}

export interface BooksListResponse {
  books: BookDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

// Request DTOs
export interface CreateBookPayload {
  isbn: string;
  title: string;
  author: string;
  totalCopies: number;
  publisher?: string | null;
  publicationYear?: number | null;
  category?: string | null;
  language?: string | null;
  price?: number | null;
  location?: string | null;
  description?: string | null;
  createdBy?: string | null;
}

export interface UpdateBookPayload {
  title?: string | null;
  author?: string | null;
  publisher?: string | null;
  publicationYear?: number | null;
  category?: string | null;
  language?: string | null;
  totalCopies?: number | null;
  price?: number | null;
  location?: string | null;
  description?: string | null;
}

export interface UpdateBookPayload {
  title?: string | null;
  author?: string | null;
  publisher?: string | null;
  publicationYear?: number | null;
  category?: string | null;
  language?: string | null;
  totalCopies?: number | null;
  price?: number | null;
  location?: string | null;
  description?: string | null;
}

export interface IssueBookPayload {
  bookId: string;
  studentId?: string | null;
  staffId?: string | null;
  issuedToType: string;
  issueDate: string;
  dueDate: string;
  issuedBy?: string | null;
  remarks?: string | null;
}

export interface ReturnBookPayload {
  issueId: string;
  returnDate: string;
  fineAmount?: number | null;
  returnedBy?: string | null;
  remarks?: string | null;
}

export interface BookIssueDto {
  id: string;
  bookId: string;
  bookTitle: string;
  bookIsbn: string;
  studentId?: string | null;
  studentName?: string | null;
  staffId?: string | null;
  staffName?: string | null;
  issuedToType: string;
  issueDate: string;
  dueDate: string;
  returnDate?: string | null;
  status: string;
  fineAmount?: number | null;
  remarks?: string | null;
  issuedBy?: string | null;
  returnedBy?: string | null;
  createdOnUtc: string;
}

export interface BookIssuesListResponse {
  issues: BookIssueDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

