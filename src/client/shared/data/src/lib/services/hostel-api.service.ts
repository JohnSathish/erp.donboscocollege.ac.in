import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';
import {
  HostelRoomDto,
  HostelRoomsListResponse,
  CreateHostelRoomPayload,
  UpdateHostelRoomPayload,
  AllocateRoomPayload,
  VacateRoomPayload,
} from '../dtos/hostel.dto';

@Injectable({ providedIn: 'root' })
export class HostelApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  listHostelRooms(params?: {
    page?: number;
    pageSize?: number;
    blockName?: string;
    roomType?: string;
    status?: string;
    searchTerm?: string;
  }): Observable<HostelRoomsListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) {
        httpParams = httpParams.set('page', params.page.toString());
      }
      if (params.pageSize) {
        httpParams = httpParams.set('pageSize', params.pageSize.toString());
      }
      if (params.blockName) {
        httpParams = httpParams.set('blockName', params.blockName);
      }
      if (params.roomType) {
        httpParams = httpParams.set('roomType', params.roomType);
      }
      if (params.status) {
        httpParams = httpParams.set('status', params.status);
      }
      if (params.searchTerm) {
        httpParams = httpParams.set('searchTerm', params.searchTerm);
      }
    }
    return this.http.get<HostelRoomsListResponse>(
      `${this.apiBaseUrl}/hostel/rooms`,
      { params: httpParams }
    );
  }

  getHostelRoom(roomId: string): Observable<HostelRoomDto> {
    return this.http.get<HostelRoomDto>(
      `${this.apiBaseUrl}/hostel/rooms/${roomId}`
    );
  }

  createHostelRoom(payload: CreateHostelRoomPayload): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/hostel/rooms`,
      payload
    );
  }

  updateHostelRoom(roomId: string, payload: UpdateHostelRoomPayload): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/hostel/rooms/${roomId}`,
      payload
    );
  }

  allocateRoom(payload: AllocateRoomPayload): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/hostel/rooms/allocate`,
      payload
    );
  }

  vacateRoom(payload: VacateRoomPayload): Observable<void> {
    return this.http.post<void>(
      `${this.apiBaseUrl}/hostel/rooms/vacate`,
      payload
    );
  }
}

