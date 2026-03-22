import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { routes } from '../../../../shared/routes/routes';

@Component({
    selector: 'app-company-settings',
    templateUrl: './company-settings.component.html',
    styleUrl: './company-settings.component.scss',
    imports: []
})
export class CompanySettingsComponent {
  public routes = routes;
  constructor(private router: Router) {}
  public onSubmit() {
    // this.router.navigate([routes.invoice]);
  }
}
