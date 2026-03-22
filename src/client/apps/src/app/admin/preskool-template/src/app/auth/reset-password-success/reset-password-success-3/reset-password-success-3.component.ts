import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
    selector: 'app-reset-password-success-3',
    templateUrl: './reset-password-success-3.component.html',
    styleUrl: './reset-password-success-3.component.scss',
   imports: [RouterLink,CommonModule]
})
export class ResetPasswordSuccess3Component {
  public routes = routes
}
