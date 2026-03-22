import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';

export interface AdminDashboardDto {
  totalApplications: number;
  submittedApplications: number;
  pendingApplications: number;
  approvedApplications: number;
  rejectedApplications: number;
  waitingListApplications: number;
  entranceExamApplications: number;
  paidApplications: number;
  unpaidApplications: number;
  pendingPipelineCount: number;
  totalRevenue: number;
  statisticsByStatus: unknown;
  offlineFormsIssued: number;
  offlineFormsReceived: number;
  offlineApplicationsSubmitted: number;
}

export interface IssueOfflineFormRequest {
  formNumber: string;
  studentName: string;
  mobileNumber: string;
  applicationFeeAmount: number;
}

export interface ReceiveOfflineFormResponse {
  accountId: string;
  formNumber: string;
  studentName: string;
  majorSubject: string;
  receivedOnUtc: string;
}

@Injectable({ providedIn: 'root' })
export class OfflineAdmissionApiService {
  constructor(
    private readonly http: HttpClient,
    @Inject(API_BASE_URL) private readonly apiBaseUrl: string
  ) {}

  private authHeaders(token: string): HttpHeaders {
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  getAdminDashboard(token: string): Observable<AdminDashboardDto> {
    return this.http.get<AdminDashboardDto>(
      `${this.apiBaseUrl}/admissions/admin/dashboard`,
      { headers: this.authHeaders(token) }
    );
  }

  issueOfflineForm(token: string, body: IssueOfflineFormRequest): Observable<Blob> {
    return this.http.post(
      `${this.apiBaseUrl}/admissions/admin/offline-forms/issue`,
      {
        formNumber: body.formNumber,
        studentName: body.studentName,
        mobileNumber: body.mobileNumber,
        applicationFeeAmount: body.applicationFeeAmount,
      },
      { headers: this.authHeaders(token), responseType: 'blob' }
    );
  }

  receiveOfflineForm(
    token: string,
    body: { formNumber: string; majorSubject: string }
  ): Observable<ReceiveOfflineFormResponse> {
    return this.http.post<ReceiveOfflineFormResponse>(
      `${this.apiBaseUrl}/admissions/admin/offline-forms/receive`,
      body,
      { headers: this.authHeaders(token) }
    );
  }

  receiptPdfUrl(formNumber: string): string {
    return `${this.apiBaseUrl}/admissions/admin/offline-forms/${formNumber}/receipt`;
  }

  assignSelectionListRound(
    token: string,
    applicationId: string,
    round: 'First' | 'Second' | 'Third'
  ): Observable<unknown> {
    return this.http.post(
      `${this.apiBaseUrl}/admissions/applications/${applicationId}/selection-list-round`,
      { round },
      { headers: this.authHeaders(token) }
    );
  }

  publishSelectionList(
    token: string,
    round: 'First' | 'Second' | 'Third',
    sendSmsNotifications: boolean
  ): Observable<unknown> {
    return this.http.post(
      `${this.apiBaseUrl}/admissions/admin/selection-list/publish`,
      { round, sendSmsNotifications },
      { headers: this.authHeaders(token) }
    );
  }

  getPublishedSelectionList(round?: 'First' | 'Second' | 'Third'): Observable<unknown[]> {
    const q = round != null ? `?round=${round}` : '';
    return this.http.get<unknown[]>(`${this.apiBaseUrl}/admissions/public/selection-list${q}`);
  }
}
