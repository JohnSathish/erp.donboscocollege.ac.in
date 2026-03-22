import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-otp-settings',
    templateUrl: './otp-settings.component.html',
    styleUrl: './otp-settings.component.scss',
    imports: [SelectModule,RouterLink]
})
export class OtpSettingsComponent {
  public routes = routes;
}
