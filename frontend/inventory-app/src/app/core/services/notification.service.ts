import { Injectable, inject } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly snackBar = inject(MatSnackBar);

  success(message: string): void {
    this.show(message, 3000, ['snackbar-success']);
  }

  error(message: string): void {
    this.show(message, 6000, ['snackbar-error']);
  }

  private show(message: string, duration: number, panelClass: string[]): void {
    const config: MatSnackBarConfig = {
      duration,
      panelClass,
      horizontalPosition: 'end',
      verticalPosition: 'top'
    };
    
    this.snackBar.open(message, 'Cerrar', config);
  }
}