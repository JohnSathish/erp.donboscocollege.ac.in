import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-ban-ip-address',
    templateUrl: './ban-ip-address.component.html',
    styleUrl: './ban-ip-address.component.scss',
    imports: [RouterLink]
})
export class BanIpAddressComponent {
  public routes = routes;
}
