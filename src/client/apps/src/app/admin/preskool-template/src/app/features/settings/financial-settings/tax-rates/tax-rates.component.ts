import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-tax-rates',
    templateUrl: './tax-rates.component.html',
    styleUrl: './tax-rates.component.scss',
    imports: [RouterLink]
})
export class TaxRatesComponent {
  public routes = routes;
}
