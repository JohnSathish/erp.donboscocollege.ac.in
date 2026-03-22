import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';
import {
  VehicleDto,
  VehiclesListResponse,
  CreateVehiclePayload,
  UpdateVehiclePayload,
} from '../dtos/transport.dto';

@Injectable({ providedIn: 'root' })
export class TransportApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  listVehicles(params?: {
    page?: number;
    pageSize?: number;
    vehicleType?: string;
    status?: string;
    searchTerm?: string;
  }): Observable<VehiclesListResponse> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.page) {
        httpParams = httpParams.set('page', params.page.toString());
      }
      if (params.pageSize) {
        httpParams = httpParams.set('pageSize', params.pageSize.toString());
      }
      if (params.vehicleType) {
        httpParams = httpParams.set('vehicleType', params.vehicleType);
      }
      if (params.status) {
        httpParams = httpParams.set('status', params.status);
      }
      if (params.searchTerm) {
        httpParams = httpParams.set('searchTerm', params.searchTerm);
      }
    }
    return this.http.get<VehiclesListResponse>(
      `${this.apiBaseUrl}/transport/vehicles`,
      { params: httpParams }
    );
  }

  getVehicle(vehicleId: string): Observable<VehicleDto> {
    return this.http.get<VehicleDto>(
      `${this.apiBaseUrl}/transport/vehicles/${vehicleId}`
    );
  }

  createVehicle(payload: CreateVehiclePayload): Observable<string> {
    return this.http.post<string>(
      `${this.apiBaseUrl}/transport/vehicles`,
      payload
    );
  }

  updateVehicle(vehicleId: string, payload: UpdateVehiclePayload): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/transport/vehicles/${vehicleId}`,
      payload
    );
  }
}

