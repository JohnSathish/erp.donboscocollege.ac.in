import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-borders',
    templateUrl: './ui-borders.component.html',
    styleUrl: './ui-borders.component.scss',
    imports: []
})
export class UiBordersComponent {
  public routes = routes;
}
