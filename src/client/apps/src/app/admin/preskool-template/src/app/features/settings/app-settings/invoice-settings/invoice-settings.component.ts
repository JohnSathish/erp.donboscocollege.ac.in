import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-invoice-settings',
    templateUrl: './invoice-settings.component.html',
    styleUrl: './invoice-settings.component.scss',
    imports: [SelectModule,RouterLink]
})
export class InvoiceSettingsComponent {
  public routes = routes;
}
