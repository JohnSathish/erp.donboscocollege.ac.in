import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { routes } from '../../../shared/routes/routes';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-login2',
    templateUrl: './login2.component.html',
    styleUrl: './login2.component.scss',
    imports: [RouterLink,CommonModule,FormsModule]
})
export class Login2Component {
  public routes = routes;
  constructor(private router: Router) {}

  public navigate() {
    this.router.navigate([routes.adminDashboard]);
  }
  public password : boolean[] = [false];

  public togglePassword(index: any){
    this.password[index] = !this.password[index]
  }
}
