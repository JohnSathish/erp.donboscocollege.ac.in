import { inject, Injectable } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { AuthService } from './auth.service';
import { ToastService } from '../shared/toast.service';
import { Observable, catchError, map, of, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuardService {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);

  canActivate(): Observable<boolean | UrlTree> {
    return this.auth.ensureAuthenticated().pipe(
      map((isAuthenticated) =>
        isAuthenticated ? true : this.router.createUrlTree(['/login'])
      ),
      tap((result) => {
        if (result !== true) {
          this.toast.show('Please sign in to continue.', 'info', 5000);
        }
      }),
      catchError(() => {
        this.toast.show('Please sign in to continue.', 'info', 5000);
        return of(this.router.createUrlTree(['/login']));
      })
    );
  }
}

export const authGuard: CanActivateFn = () =>
  inject(AuthGuardService).canActivate();

/** Use for admin routes: redirects to /AdminLogin instead of /login when not authenticated. */
@Injectable({ providedIn: 'root' })
export class AdminAuthGuardService {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);

  canActivate(): Observable<boolean | UrlTree> {
    return this.auth.ensureAuthenticated().pipe(
      map((isAuthenticated) =>
        isAuthenticated ? true : this.router.createUrlTree(['/AdminLogin'])
      ),
      tap((result) => {
        if (result !== true) {
          this.toast.show('Please sign in to access the admin panel.', 'info', 5000);
        }
      }),
      catchError(() => {
        this.toast.show('Please sign in to access the admin panel.', 'info', 5000);
        return of(this.router.createUrlTree(['/AdminLogin']));
      })
    );
  }
}

export const adminAuthGuard: CanActivateFn = () =>
  inject(AdminAuthGuardService).canActivate();
