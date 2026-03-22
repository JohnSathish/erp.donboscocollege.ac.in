import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-modals',
    templateUrl: './ui-modals.component.html',
    styleUrl: './ui-modals.component.scss',
    imports: []
})
export class UiModalsComponent {
  public routes = routes;
}
