import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-placeholders',
    templateUrl: './ui-placeholders.component.html',
    styleUrl: './ui-placeholders.component.scss',
    imports: []
})
export class UiPlaceholdersComponent {
  public routes = routes;
}
