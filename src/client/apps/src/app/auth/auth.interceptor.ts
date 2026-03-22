import { inject, Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private readonly auth = inject(AuthService);

  intercept(
    req: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const profile = this.auth.profile;
    const isAuthRequest = req.url.includes('/auth/applicants');

    const authReq =
      profile && !isAuthRequest && profile.token
        ? req.clone({
            setHeaders: { Authorization: `Bearer ${profile.token}` },
          })
        : req;

    return next.handle(authReq).pipe(
      catchError((error) => {
        if (error.status === 401 && profile?.refreshToken) {
          return this.auth
            .refreshToken(profile.refreshToken)
            .pipe(
              switchMap(() => {
                const updatedToken = this.auth.profile?.token;
                if (!updatedToken) {
                  this.auth.logout();
                  return throwError(() => error);
                }
                const retryRequest = req.clone({
                  setHeaders: { Authorization: `Bearer ${updatedToken}` },
                });
                return next.handle(retryRequest);
              })
            );
        }

        if (error.status === 401 || error.status === 403) {
          this.auth.logout();
        }

        return throwError(() => error);
      })
    );
  }
}
