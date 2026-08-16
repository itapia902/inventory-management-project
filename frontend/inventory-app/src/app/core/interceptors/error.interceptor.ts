import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notification = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      notification.error(extractMessage(error));
      return throwError(() => error);
    })
  );
};

function extractMessage(error: HttpErrorResponse): string {
  if (error.status === 0) {
    return 'No se pudo conectar con el servidor. Verifique que los servicios estén ejecutándose.';
  }

  const backendError = error.error;

  if (backendError?.errors) {
    const validationMessages = Object.values<string[]>(backendError.errors).flat();
    
    if (validationMessages.length > 0) {
      return validationMessages.join(' ');
    }
  }

  if (backendError?.title) {
    return backendError.title;
  }
  return 'Ha ocurrido un error inesperado.';
}