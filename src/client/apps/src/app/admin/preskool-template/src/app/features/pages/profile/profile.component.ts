import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrl: './profile.component.scss',
    imports: [CommonModule,RouterLink]
})
export class ProfileComponent {
  public routes = routes;
  isPassword = false;
  isPassword2 = false;
  isPassword3 = false;
  isPassword4 = false;

  togglePassword() {
    this.isPassword = !this.isPassword;
  }
  togglePassword2() {
    this.isPassword = !this.isPassword;
  }
  togglePassword3() {
    this.isPassword = !this.isPassword;
  }
  togglePassword4() {
    this.isPassword = !this.isPassword;
  }
}
