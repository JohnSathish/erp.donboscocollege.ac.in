import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  ApplicantApplicationDraft,
  ApplicantApplicationDraftResponse,
  ClassXiiSubjectOptionDto,
} from '../dtos/applicant-application.dto';
import { API_BASE_URL } from '@client/shared/util';

@Injectable({ providedIn: 'root' })
export class ApplicantApplicationApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  loadDraft(): Observable<ApplicantApplicationDraftResponse> {
    return this.http.get<ApplicantApplicationDraftResponse>(
      `${this.apiBaseUrl}/applicant-applications/me`
    );
  }

  /** Anonymous: subject catalog for Class XII (not for OTHER board). */
  getClassXiiSubjects(
    board: string,
    stream: string
  ): Observable<{ items: ClassXiiSubjectOptionDto[] }> {
    return this.http.get<{ items: ClassXiiSubjectOptionDto[] }>(
      `${this.apiBaseUrl}/admissions/class-xii-subjects`,
      { params: { board, stream } }
    );
  }

  saveDraft(
    payload: ApplicantApplicationDraft
  ): Observable<ApplicantApplicationDraftResponse> {
    return this.http.post<ApplicantApplicationDraftResponse>(
      `${this.apiBaseUrl}/applicant-applications/me`,
      payload
    );
  }

  submitApplication(
    payload: ApplicantApplicationDraft
  ): Observable<HttpResponse<Blob>> {
    return this.http.post(`${this.apiBaseUrl}/applicant-applications/me/submit`, payload, {
      observe: 'response',
      responseType: 'blob',
    });
  }

  uploadDocument(
    documentType: string,
    file: File
  ): Observable<UploadDocumentResult> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.http.post<UploadDocumentResult>(
      `${this.apiBaseUrl}/applicant-applications/me/documents/${documentType}`,
      formData
    );
  }

  getOffer(): Observable<AdmissionOfferDto> {
    return this.http.get<AdmissionOfferDto>(
      `${this.apiBaseUrl}/applicant-applications/me/offer`
    );
  }

  acceptOffer(): Observable<AcceptOfferResponse> {
    return this.http.post<AcceptOfferResponse>(
      `${this.apiBaseUrl}/applicant-applications/me/offer/accept`,
      {}
    );
  }

  rejectOffer(reason?: string): Observable<RejectOfferResponse> {
    return this.http.post<RejectOfferResponse>(
      `${this.apiBaseUrl}/applicant-applications/me/offer/reject`,
      { reason }
    );
  }
}

export interface UploadDocumentResult {
  documentType: string;
  fileName: string;
  fileSizeBytes: number;
  uploadedOnUtc: string;
}

export interface AdmissionOfferDto {
  id: string;
  applicationNumber: string;
  fullName: string;
  meritRank: number;
  shift: string;
  majorSubject: string;
  status: string;
  offerDate: string;
  expiryDate: string;
  acceptedOnUtc?: string | null;
  rejectedOnUtc?: string | null;
  remarks?: string | null;
}

export interface AcceptOfferResponse {
  offerId: string;
  applicationNumber: string;
  success: boolean;
  message: string;
}

export interface RejectOfferResponse {
  offerId: string;
  applicationNumber: string;
  success: boolean;
  message: string;
}

