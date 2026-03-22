import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-sms-settings',
    templateUrl: './sms-settings.component.html',
    styleUrl: './sms-settings.component.scss',
    imports: [RouterLink]
})
export class SmsSettingsComponent {
  public routes = routes;
}
