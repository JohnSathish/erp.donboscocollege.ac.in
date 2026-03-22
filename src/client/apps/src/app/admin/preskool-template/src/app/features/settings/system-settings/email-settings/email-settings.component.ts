import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-email-settings',
    templateUrl: './email-settings.component.html',
    styleUrl: './email-settings.component.scss',
    imports: [RouterLink]
})
export class EmailSettingsComponent {
  public routes = routes;
}
