import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '@client/shared/util';
import {
  RegisterStudentApplicantRequest,
  RegisterStudentApplicantResponse,
} from '../dtos/register-student-applicant.dto';

@Injectable({ providedIn: 'root' })
export class RegistrationApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  registerApplicant(
    payload: RegisterStudentApplicantRequest,
    profilePhoto?: File
  ): Observable<RegisterStudentApplicantResponse> {
    const formData = new FormData();
    formData.append('FullName', payload.fullName);
    formData.append('DateOfBirth', payload.dateOfBirth);
    formData.append('Gender', payload.gender);
    formData.append('Email', payload.email);
    formData.append('MobileNumber', payload.mobileNumber);

    if (profilePhoto) {
      formData.append('ProfilePhoto', profilePhoto, profilePhoto.name);
    }

    return this.http.post<RegisterStudentApplicantResponse>(
      `${this.apiBaseUrl}/registration/student`,
      formData
    );
  }
}
