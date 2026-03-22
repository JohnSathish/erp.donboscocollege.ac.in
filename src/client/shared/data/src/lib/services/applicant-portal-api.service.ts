import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApplicantDashboardDto } from '../dtos/applicant-dashboard.dto';
import { API_BASE_URL } from '@client/shared/util';

@Injectable({ providedIn: 'root' })
export class ApplicantPortalApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  getDashboard(): Observable<ApplicantDashboardDto> {
    return this.http.get<ApplicantDashboardDto>(
      `${this.apiBaseUrl}/applicant-portal/me`
    );
  }
}
