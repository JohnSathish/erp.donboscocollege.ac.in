import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { Router } from '@angular/router';

@Component({
    selector: 'app-preferences',
    templateUrl: './preferences.component.html',
    styleUrl: './preferences.component.scss',
    imports: []
})
export class PreferencesComponent {
  public routes = routes;
  constructor(private router: Router) {}
  public onSubmit() {
    // this.router.navigate([routes.invoice]);
  }
}
