import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-payment-gateways',
    templateUrl: './payment-gateways.component.html',
    styleUrl: './payment-gateways.component.scss',
    imports: [RouterLink]
})
export class PaymentGatewaysComponent {
  public routes = routes;
}
