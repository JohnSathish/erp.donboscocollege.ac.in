import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, map, of, tap } from 'rxjs';
import { Router } from '@angular/router';
import {
  AdminLoginResponse,
  ChangePasswordRequest,
  LoginRequest,
  LoginResponse,
  RefreshRequest,
} from './auth.dto';
import { API_BASE_URL } from '@client/shared/util';

export interface AuthState {
  profile: LoginResponse | null;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  private readonly storageKey = 'dbct-erp-auth';
  private shouldPersist = false;

  private readonly state$ = new BehaviorSubject<AuthState>({
    profile: this.loadStoredProfile(),
  });

  readonly authState$ = this.state$.asObservable();

  constructor() {
    if (this.state$.value.profile) {
      this.shouldPersist = true;
    }
  }

  get profile(): LoginResponse | null {
    return this.state$.value.profile;
  }

  login(payload: LoginRequest, remember = false): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiBaseUrl}/auth/applicants/login`, payload)
      .pipe(
        tap((response) => {
          this.shouldPersist = remember;
          this.persistProfile(response);
        })
      );
  }

  loginAdmin(payload: LoginRequest, remember = false): Observable<LoginResponse> {
    return this.http
      .post<AdminLoginResponse>(`${this.apiBaseUrl}/auth/admin/login`, payload)
      .pipe(
        map((adminResponse): LoginResponse => {
          const response: LoginResponse = {
            token: adminResponse.token,
            expiresAtUtc: adminResponse.expiresAtUtc,
            uniqueId: adminResponse.uniqueId,
            email: adminResponse.email,
            fullName: adminResponse.fullName,
          };
          return response;
        }),
        tap((response) => {
          this.shouldPersist = remember;
          this.persistProfile(response);
        })
      );
  }

  refreshToken(refreshToken: string): Observable<LoginResponse> {
    const payload: RefreshRequest = { refreshToken };
    return this.http
      .post<LoginResponse>(`${this.apiBaseUrl}/auth/applicants/refresh`, payload)
      .pipe(tap((response) => this.persistProfile(response)));
  }

  changePassword(payload: ChangePasswordRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.apiBaseUrl}/auth/applicants/change-password`,
        payload
      )
      .pipe(tap((response) => this.persistProfile(response)));
  }

  ensureAuthenticated(): Observable<boolean> {
    const profile = this.state$.value.profile ?? this.restoreSessionFromStorage();
    if (!profile) {
      return of(false);
    }

    if (this.isTokenValid(profile.expiresAtUtc)) {
      return of(true);
    }

    // Admin users don't have refresh tokens, so if token is expired, they need to login again
    const refreshToken = profile.refreshToken;
    if (!refreshToken) {
      this.clearSession();
      return of(false);
    }

    return this.refreshToken(refreshToken).pipe(
      map(() => true),
      catchError(() => {
        this.clearSession();
        return of(false);
      })
    );
  }

  /** @param redirectTo Defaults to '/login' (applicant). Use '/AdminLogin' when logging out from admin. */
  logout(redirectTo = '/login'): void {
    this.clearSession();
    this.router.navigate([redirectTo]);
  }

  private persistProfile(response: LoginResponse): void {
    const profile: LoginResponse = {
      ...response,
      expiresAtUtc: response.expiresAtUtc,
      refreshToken: response.refreshToken,
      refreshTokenExpiresAtUtc: response.refreshTokenExpiresAtUtc,
      mustChangePassword: response.mustChangePassword,
    };

    this.state$.next({ profile });

    if (this.shouldPersist) {
      localStorage.setItem(this.storageKey, JSON.stringify(profile));
    } else {
      localStorage.removeItem(this.storageKey);
    }
  }

  private loadStoredProfile(): LoginResponse | null {
    const raw = localStorage.getItem(this.storageKey);
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as LoginResponse;
    } catch {
      localStorage.removeItem(this.storageKey);
      return null;
    }
  }

  private restoreSessionFromStorage(): LoginResponse | null {
    const stored = this.loadStoredProfile();
    if (stored) {
      this.shouldPersist = true;
      this.state$.next({ profile: stored });
      return stored;
    }

    return null;
  }

  private clearSession(): void {
    this.state$.next({ profile: null });
    this.shouldPersist = false;
    localStorage.removeItem(this.storageKey);
  }

  private isTokenValid(expiresAtUtc: string, skewSeconds = 60): boolean {
    // Treat as UTC if no timezone suffix (backend often sends DateTime.UtcNow without 'Z')
    const utcString =
      typeof expiresAtUtc === 'string' && !/Z$/i.test(expiresAtUtc.trim())
        ? expiresAtUtc.trim() + 'Z'
        : expiresAtUtc;
    const expires = new Date(utcString).getTime();
    if (Number.isNaN(expires)) {
      return false;
    }

    const now = Date.now();
    return expires - skewSeconds * 1000 > now;
  }
}
