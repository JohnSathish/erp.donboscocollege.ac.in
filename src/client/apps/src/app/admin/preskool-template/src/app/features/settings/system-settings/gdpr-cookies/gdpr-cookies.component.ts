import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-gdpr-cookies',
    templateUrl: './gdpr-cookies.component.html',
    styleUrl: './gdpr-cookies.component.scss',
    imports: [SelectModule,RouterLink]
})
export class GdprCookiesComponent {
  public routes = routes;
}
