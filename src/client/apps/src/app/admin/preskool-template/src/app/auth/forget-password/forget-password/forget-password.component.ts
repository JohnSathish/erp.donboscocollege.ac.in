import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-forget-password',
    templateUrl: './forget-password.component.html',
    styleUrl: './forget-password.component.scss',
    imports: [RouterLink,CommonModule]
})
export class ForgetPasswordComponent {
  public routes = routes
  constructor(private router: Router) {}

  public navigate() {
    this.router.navigate([routes.login]);
  }
}
