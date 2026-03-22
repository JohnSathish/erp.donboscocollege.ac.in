import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';
import {
  StaffMemberDto,
  StaffMembersListResponse,
  CreateStaffMemberPayload,
  UpdateStaffMemberPayload,
} from '../dtos/staff.dto';

@Injectable({ providedIn: 'root' })
export class StaffApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  listStaffMembers(params?: {
    page?: number;
    pageSize?: number;
    department?: string;
    employeeType?: string;
    status?: string;
    searchTerm?: string;
  }): Observable<StaffMembersListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) {
        httpParams = httpParams.set('page', params.page.toString());
      }
      if (params.pageSize) {
        httpParams = httpParams.set('pageSize', params.pageSize.toString());
      }
      if (params.department) {
        httpParams = httpParams.set('department', params.department);
      }
      if (params.employeeType) {
        httpParams = httpParams.set('employeeType', params.employeeType);
      }
      if (params.status) {
        httpParams = httpParams.set('status', params.status);
      }
      if (params.searchTerm) {
        httpParams = httpParams.set('searchTerm', params.searchTerm);
      }
    }
    return this.http.get<StaffMembersListResponse>(
      `${this.apiBaseUrl}/staff`,
      { params: httpParams }
    );
  }

  getStaffMember(staffId: string): Observable<StaffMemberDto> {
    return this.http.get<StaffMemberDto>(
      `${this.apiBaseUrl}/staff/${staffId}`
    );
  }

  createStaffMember(payload: CreateStaffMemberPayload): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/staff`,
      payload
    );
  }

  updateStaffMember(
    staffId: string,
    payload: UpdateStaffMemberPayload
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/staff/${staffId}`,
      payload
    );
  }
}

