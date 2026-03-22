import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';

@Component({
    selector: 'app-notifications-settings',
    templateUrl: './notifications-settings.component.html',
    styleUrl: './notifications-settings.component.scss',
    imports: []
})
export class NotificationsSettingsComponent {
  public routes = routes;
}
