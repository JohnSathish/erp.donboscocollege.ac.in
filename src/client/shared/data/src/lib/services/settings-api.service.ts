import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';
import {
  ProgramDto,
  ProgramsListResponse,
  CourseDto,
  CoursesListResponse,
  FeeStructureDto,
  FeeStructuresListResponse,
  FeeComponentDto,
} from '../dtos/settings.dto';

@Injectable({ providedIn: 'root' })
export class SettingsApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  // ========== Programs ==========
  listPrograms(params?: {
    page?: number;
    pageSize?: number;
    isActive?: boolean;
    searchTerm?: string;
  }): Observable<ProgramsListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) httpParams = httpParams.set('page', params.page);
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize);
      if (params.isActive !== undefined) httpParams = httpParams.set('isActive', params.isActive);
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    }
    return this.http.get<ProgramsListResponse>(`${this.apiBaseUrl}/settings/programs`, { params: httpParams });
  }

  getProgram(id: string): Observable<ProgramDto> {
    return this.http.get<ProgramDto>(`${this.apiBaseUrl}/settings/programs/${id}`);
  }

  createProgram(payload: CreateProgramPayload): Observable<string> {
    return this.http.post<string>(`${this.apiBaseUrl}/settings/programs`, payload);
  }

  updateProgram(id: string, payload: UpdateProgramPayload): Observable<void> {
    return this.http.put<void>(`${this.apiBaseUrl}/settings/programs/${id}`, payload);
  }

  deleteProgram(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/settings/programs/${id}`);
  }

  toggleProgramStatus(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiBaseUrl}/settings/programs/${id}/status`, {});
  }

  // ========== Courses ==========
  listCourses(params?: {
    page?: number;
    pageSize?: number;
    isActive?: boolean;
    programId?: string;
    searchTerm?: string;
  }): Observable<CoursesListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) httpParams = httpParams.set('page', params.page);
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize);
      if (params.isActive !== undefined) httpParams = httpParams.set('isActive', params.isActive);
      if (params.programId) httpParams = httpParams.set('programId', params.programId);
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    }
    return this.http.get<CoursesListResponse>(`${this.apiBaseUrl}/settings/courses`, { params: httpParams });
  }

  getCourse(id: string): Observable<CourseDto> {
    return this.http.get<CourseDto>(`${this.apiBaseUrl}/settings/courses/${id}`);
  }

  createCourse(payload: CreateCoursePayload): Observable<string> {
    return this.http.post<string>(`${this.apiBaseUrl}/settings/courses`, payload);
  }

  updateCourse(id: string, payload: UpdateCoursePayload): Observable<void> {
    return this.http.put<void>(`${this.apiBaseUrl}/settings/courses/${id}`, payload);
  }

  deleteCourse(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/settings/courses/${id}`);
  }

  toggleCourseStatus(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiBaseUrl}/settings/courses/${id}/status`, {});
  }

  // ========== Fee Structures ==========
  listFeeStructures(params?: {
    page?: number;
    pageSize?: number;
    isActive?: boolean;
    programId?: string;
    academicYear?: string;
    searchTerm?: string;
  }): Observable<FeeStructuresListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) httpParams = httpParams.set('page', params.page);
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize);
      if (params.isActive !== undefined) httpParams = httpParams.set('isActive', params.isActive);
      if (params.programId) httpParams = httpParams.set('programId', params.programId);
      if (params.academicYear) httpParams = httpParams.set('academicYear', params.academicYear);
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    }
    return this.http.get<FeeStructuresListResponse>(`${this.apiBaseUrl}/settings/fee-structures`, { params: httpParams });
  }

  getFeeStructure(id: string): Observable<FeeStructureDto> {
    return this.http.get<FeeStructureDto>(`${this.apiBaseUrl}/settings/fee-structures/${id}`);
  }

  createFeeStructure(payload: CreateFeeStructurePayload): Observable<string> {
    return this.http.post<string>(`${this.apiBaseUrl}/settings/fee-structures`, payload);
  }

  updateFeeStructure(id: string, payload: UpdateFeeStructurePayload): Observable<void> {
    return this.http.put<void>(`${this.apiBaseUrl}/settings/fee-structures/${id}`, payload);
  }

  deleteFeeStructure(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/settings/fee-structures/${id}`);
  }

  toggleFeeStructureStatus(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiBaseUrl}/settings/fee-structures/${id}/status`, {});
  }

  // ========== Fee Components ==========
  addFeeComponent(feeStructureId: string, payload: AddFeeComponentPayload): Observable<string> {
    return this.http.post<string>(`${this.apiBaseUrl}/settings/fee-structures/${feeStructureId}/components`, payload);
  }

  updateFeeComponent(id: string, payload: UpdateFeeComponentPayload): Observable<void> {
    return this.http.put<void>(`${this.apiBaseUrl}/settings/fee-components/${id}`, payload);
  }

  deleteFeeComponent(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/settings/fee-components/${id}`);
  }
}

// Payload Types
export interface CreateProgramPayload {
  code: string;
  name: string;
  level: string;
  durationYears: number;
  totalCredits: number;
  description?: string | null;
}

export interface UpdateProgramPayload {
  name: string;
  level: string;
  durationYears: number;
  totalCredits: number;
  description?: string | null;
}

export interface CreateCoursePayload {
  code: string;
  name: string;
  creditHours: number;
  programId?: string | null;
  description?: string | null;
  prerequisites?: string | null;
}

export interface UpdateCoursePayload {
  name: string;
  creditHours: number;
  programId?: string | null;
  description?: string | null;
  prerequisites?: string | null;
}

export interface CreateFeeStructurePayload {
  name: string;
  academicYear: string;
  effectiveFromUtc: string;
  programId?: string | null;
  description?: string | null;
  effectiveToUtc?: string | null;
  components?: FeeComponentRequestDto[] | null;
}

export interface FeeComponentRequestDto {
  name: string;
  amount: number;
  isOptional?: boolean;
  installmentNumber?: number | null;
  dueDateUtc?: string | null;
  description?: string | null;
  displayOrder?: number;
}

export interface UpdateFeeStructurePayload {
  name: string;
  academicYear: string;
  effectiveFromUtc: string;
  programId?: string | null;
  description?: string | null;
  effectiveToUtc?: string | null;
}

export interface AddFeeComponentPayload {
  name: string;
  amount: number;
  isOptional?: boolean;
  installmentNumber?: number | null;
  dueDateUtc?: string | null;
  description?: string | null;
  displayOrder?: number;
}

export interface UpdateFeeComponentPayload {
  name: string;
  amount: number;
  isOptional?: boolean;
  installmentNumber?: number | null;
  dueDateUtc?: string | null;
  description?: string | null;
  displayOrder?: number;
}


