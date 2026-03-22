import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-reset-password-2',
    templateUrl: './reset-password-2.component.html',
    styleUrl: './reset-password-2.component.scss',
   imports: [RouterLink,CommonModule]
})
export class ResetPassword2Component {
  public routes = routes;
  public password : boolean[] = [false];

  public togglePassword(index: any){
    this.password[index] = !this.password[index]
  }
  constructor(private router: Router) {}

  navigation() {
    this.router.navigate([routes.success2])
  }
}
