import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import {  RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
    selector: 'app-reset-password-success-2',
    templateUrl: './reset-password-success-2.component.html',
    styleUrl: './reset-password-success-2.component.scss',
   imports: [RouterLink,CommonModule]
})
export class ResetPasswordSuccess2Component {
  public routes = routes
}
