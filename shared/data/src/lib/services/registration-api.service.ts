import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  RegisterStudentApplicantRequest,
  RegisterStudentApplicantResponse,
} from '../dtos/register-student-applicant.dto';
import { API_BASE_URL } from '@client/shared/util';

@Injectable({
  providedIn: 'root',
})
export class RegistrationApiService {
  constructor(
    private readonly http: HttpClient,
    @Inject(API_BASE_URL) private readonly apiBaseUrl: string
  ) {}

  /**
   * Posts multipart/form-data — API expects `ProfilePhoto` (file) and PascalCase fields
   * (see `StudentRegistrationController`).
   */
  registerApplicant(
    payload: RegisterStudentApplicantRequest,
    profilePhoto: File
  ): Observable<RegisterStudentApplicantResponse> {
    const fd = new FormData();
    fd.append('FullName', payload.fullName);
    fd.append('DateOfBirth', payload.dateOfBirth);
    fd.append('Gender', payload.gender);
    fd.append('Email', payload.email);
    fd.append('MobileNumber', payload.mobileNumber);
    fd.append('ProfilePhoto', profilePhoto, profilePhoto.name);
    return this.http.post<RegisterStudentApplicantResponse>(
      `${this.apiBaseUrl}/registration/student`,
      fd
    );
  }
}

