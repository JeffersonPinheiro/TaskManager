import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { throwError, switchMap, catchError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  const token = authService.getToken();
  
  // Add authorization header if token exists
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        // Try to refresh token
        return authService.refreshToken().pipe(
          switchMap(() => {
            // Retry the original request with new token
            const newToken = authService.getToken();
            const retryReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`
              }
            });
            return next(retryReq);
          }),
          catchError(() => {
            // Refresh failed, logout and redirect to login
            authService.logout();
            router.navigate(['/login']);
            return throwError(() => error);
          })
        );
      }
      return throwError(() => error);
    })
  );
};