import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';

export interface AcademicTermDto {
  id: string;
  termName: string;
  academicYear: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
  remarks?: string;
}

export interface CreateAcademicTermPayload {
  termName: string;
  academicYear: string;
  startDate: string; // ISO date string
  endDate: string; // ISO date string
  remarks?: string;
}

@Injectable({ providedIn: 'root' })
export class TimetableApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  createAcademicTerm(payload: CreateAcademicTermPayload): Observable<string> {
    return this.http.post<string>(`${this.apiBaseUrl}/Timetable/terms`, payload);
  }

  listAcademicTerms(params?: {
    academicYear?: string;
    isActive?: boolean;
  }): Observable<AcademicTermDto[]> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.academicYear) httpParams = httpParams.set('academicYear', params.academicYear);
      if (params.isActive !== undefined) httpParams = httpParams.set('isActive', params.isActive);
    }
    return this.http.get<AcademicTermDto[]>(`${this.apiBaseUrl}/Timetable/terms`, { params: httpParams });
  }

  activateAcademicTerm(termId: string): Observable<void> {
    return this.http.post<void>(`${this.apiBaseUrl}/Timetable/terms/${termId}/activate`, {});
  }

  deactivateAcademicTerm(termId: string): Observable<void> {
    return this.http.post<void>(`${this.apiBaseUrl}/Timetable/terms/${termId}/deactivate`, {});
  }
}

