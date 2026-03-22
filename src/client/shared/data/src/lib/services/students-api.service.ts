import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';
import {
  StudentDto,
  StudentsListResponse,
  UpdateStudentProfilePayload,
  UpdateStudentStatusPayload,
  ConvertApplicantToStudentPayload,
} from '../dtos/students.dto';

@Injectable({ providedIn: 'root' })
export class StudentsApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  listStudents(params?: {
    page?: number;
    pageSize?: number;
    isActive?: boolean;
    programId?: string;
    academicYear?: string;
    searchTerm?: string;
  }): Observable<StudentsListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) {
        httpParams = httpParams.set('page', params.page.toString());
      }
      if (params.pageSize) {
        httpParams = httpParams.set('pageSize', params.pageSize.toString());
      }
      if (params.isActive !== undefined && params.isActive !== null) {
        httpParams = httpParams.set('isActive', params.isActive.toString());
      }
      if (params.programId) {
        httpParams = httpParams.set('programId', params.programId);
      }
      if (params.academicYear) {
        httpParams = httpParams.set('academicYear', params.academicYear);
      }
      if (params.searchTerm) {
        httpParams = httpParams.set('searchTerm', params.searchTerm);
      }
    }
    return this.http.get<StudentsListResponse>(
      `${this.apiBaseUrl}/students`,
      { params: httpParams }
    );
  }

  getStudent(studentId: string): Observable<StudentDto> {
    return this.http.get<StudentDto>(
      `${this.apiBaseUrl}/students/${studentId}`
    );
  }

  updateStudentProfile(
    studentId: string,
    payload: UpdateStudentProfilePayload
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/students/${studentId}/profile`,
      payload
    );
  }

  updateStudentStatus(
    studentId: string,
    payload: UpdateStudentStatusPayload
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/students/${studentId}/status`,
      payload
    );
  }

  convertApplicantToStudent(
    payload: ConvertApplicantToStudentPayload
  ): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/students/convert-from-applicant`,
      payload
    );
  }
}




