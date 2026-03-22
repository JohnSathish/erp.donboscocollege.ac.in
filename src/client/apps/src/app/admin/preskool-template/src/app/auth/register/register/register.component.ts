import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { routes } from '../../../shared/routes/routes';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.scss',
   imports: [RouterLink,CommonModule]
})
export class RegisterComponent {
  public routes = routes
  constructor(private router: Router) {}

  public navigate() {
    this.router.navigate([routes.login]);
  }
  public password : boolean[] = [false];

  public togglePassword(index: any){
    this.password[index] = !this.password[index]
  }
}
