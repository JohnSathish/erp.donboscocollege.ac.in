import { of, throwError } from 'rxjs';
import { authGuard } from './auth.guard';
import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { ToastService } from '../shared/toast.service';

describe('authGuard', () => {
  const createUrlTree = jest.fn(
    () => 'login-url-tree' as unknown as UrlTree
  );
  const ensureAuthenticated = jest.fn();
  const showToast = jest.fn();

  beforeEach(() => {
    createUrlTree.mockClear();
    ensureAuthenticated.mockReset();
    showToast.mockReset();

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: { ensureAuthenticated } },
        { provide: Router, useValue: { createUrlTree } },
        { provide: ToastService, useValue: { show: showToast } },
      ],
    });
  });

  it('allows navigation when already authenticated', (done) => {
    ensureAuthenticated.mockReturnValue(of(true));

    TestBed.runInInjectionContext(() => {
      authGuard(
        {} as ActivatedRouteSnapshot,
        {} as RouterStateSnapshot
      ).subscribe((result) => {
        expect(result).toBe(true);
        expect(showToast).not.toHaveBeenCalled();
        done();
      });
    });
  });

  it('redirects to login when authentication fails', (done) => {
    ensureAuthenticated.mockReturnValue(of(false));

    TestBed.runInInjectionContext(() => {
      authGuard(
        {} as ActivatedRouteSnapshot,
        {} as RouterStateSnapshot
      ).subscribe((result) => {
        expect(result).toBe('login-url-tree');
        expect(createUrlTree).toHaveBeenCalledWith(['/login']);
        expect(showToast).toHaveBeenCalled();
        done();
      });
    });
  });

  it('redirects to login when authentication throws', (done) => {
    ensureAuthenticated.mockReturnValue(throwError(() => new Error('fail')));

    TestBed.runInInjectionContext(() => {
      authGuard(
        {} as ActivatedRouteSnapshot,
        {} as RouterStateSnapshot
      ).subscribe((result) => {
        expect(result).toBe('login-url-tree');
        expect(showToast).toHaveBeenCalled();
        done();
      });
    });
  });
});
