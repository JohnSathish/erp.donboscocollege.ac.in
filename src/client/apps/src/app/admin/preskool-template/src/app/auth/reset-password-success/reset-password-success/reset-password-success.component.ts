import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import {  RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
    selector: 'app-reset-password-success',
    templateUrl: './reset-password-success.component.html',
    styleUrl: './reset-password-success.component.scss',
   imports: [RouterLink,CommonModule]
})
export class ResetPasswordSuccessComponent {
  public routes = routes
}
