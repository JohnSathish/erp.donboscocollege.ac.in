import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-blank-page',
    templateUrl: './blank-page.component.html',
    styleUrl: './blank-page.component.scss',
    imports: [RouterLink]
})
export class BlankPageComponent {
  public routes = routes;
}
