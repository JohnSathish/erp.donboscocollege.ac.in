import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-typography',
    templateUrl: './ui-typography.component.html',
    styleUrl: './ui-typography.component.scss',
    imports: []
})
export class UiTypographyComponent {
  public routes = routes;
}
