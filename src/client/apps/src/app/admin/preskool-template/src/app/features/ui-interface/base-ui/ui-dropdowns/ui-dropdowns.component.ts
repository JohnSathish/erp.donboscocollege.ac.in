import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-dropdowns',
    templateUrl: './ui-dropdowns.component.html',
    styleUrl: './ui-dropdowns.component.scss',
    imports: []
})
export class UiDropdownsComponent {
  public routes = routes;
}
