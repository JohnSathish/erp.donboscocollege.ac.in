import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-email-verification-3',
    templateUrl: './email-verification-3.component.html',
    styleUrl: './email-verification-3.component.scss',
    imports: [CommonModule]
})
export class EmailVerification3Component {
  public routes = routes;
  constructor(private router: Router) {}

  public navigate() {
    this.router.navigate([routes.login3]);
  }
}
