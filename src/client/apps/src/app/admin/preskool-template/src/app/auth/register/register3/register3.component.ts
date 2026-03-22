import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
    selector: 'app-register3',
    templateUrl: './register3.component.html',
    styleUrl: './register3.component.scss',
   imports: [RouterLink,CommonModule]
})
export class Register3Component {
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
