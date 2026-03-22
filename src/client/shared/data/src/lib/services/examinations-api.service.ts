import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';
import {
  AssessmentDto,
  AssessmentSummaryDto,
  MarkEntryDto,
  ResultSummaryDto,
  CreateAssessmentPayload,
  EnterMarksPayload,
  BulkEnterMarksPayload,
  PublishAssessmentPayload,
  ApproveMarksPayload,
  ProcessResultsPayload,
} from '../dtos/examinations.dto';

@Injectable({ providedIn: 'root' })
export class ExaminationsApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  // Assessment endpoints
  listAssessments(params?: {
    courseId?: string;
    academicTermId?: string;
    classSectionId?: string;
    status?: string;
  }): Observable<AssessmentSummaryDto[]> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.courseId) {
        httpParams = httpParams.set('courseId', params.courseId);
      }
      if (params.academicTermId) {
        httpParams = httpParams.set('academicTermId', params.academicTermId);
      }
      if (params.classSectionId) {
        httpParams = httpParams.set('classSectionId', params.classSectionId);
      }
      if (params.status) {
        httpParams = httpParams.set('status', params.status);
      }
    }
    return this.http.get<AssessmentSummaryDto[]>(
      `${this.apiBaseUrl}/examinations/assessments`,
      { params: httpParams }
    );
  }

  getAssessment(assessmentId: string): Observable<AssessmentDto> {
    return this.http.get<AssessmentDto>(
      `${this.apiBaseUrl}/examinations/assessments/${assessmentId}`
    );
  }

  createAssessment(payload: CreateAssessmentPayload): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/examinations/assessments`,
      payload
    );
  }

  publishAssessment(
    assessmentId: string,
    payload?: PublishAssessmentPayload
  ): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.apiBaseUrl}/examinations/assessments/${assessmentId}/publish`,
      payload || {}
    );
  }

  // Mark Entry endpoints
  enterMarks(payload: EnterMarksPayload): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/examinations/marks`,
      payload
    );
  }

  bulkEnterMarks(payload: BulkEnterMarksPayload): Observable<{
    count: number;
    message: string;
  }> {
    return this.http.post<{ count: number; message: string }>(
      `${this.apiBaseUrl}/examinations/marks/bulk`,
      payload
    );
  }

  getStudentMarks(
    studentId: string,
    academicTermId?: string
  ): Observable<MarkEntryDto[]> {
    let httpParams = new HttpParams();
    if (academicTermId) {
      httpParams = httpParams.set('academicTermId', academicTermId);
    }
    return this.http.get<MarkEntryDto[]>(
      `${this.apiBaseUrl}/examinations/marks/student/${studentId}`,
      { params: httpParams }
    );
  }

  approveMarks(
    markEntryId: string,
    payload?: ApproveMarksPayload
  ): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.apiBaseUrl}/examinations/marks/${markEntryId}/approve`,
      payload || {}
    );
  }

  // Results endpoints
  processResults(payload: ProcessResultsPayload): Observable<{
    resultSummaryId: string;
    message: string;
  }> {
    return this.http.post<{ resultSummaryId: string; message: string }>(
      `${this.apiBaseUrl}/examinations/results/process`,
      payload
    );
  }

  getResults(
    studentId: string,
    academicTermId: string
  ): Observable<ResultSummaryDto> {
    return this.http.get<ResultSummaryDto>(
      `${this.apiBaseUrl}/examinations/results/student/${studentId}/term/${academicTermId}`
    );
  }
}




