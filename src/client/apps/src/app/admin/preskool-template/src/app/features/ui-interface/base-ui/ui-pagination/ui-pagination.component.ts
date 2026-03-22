import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-pagination',
    templateUrl: './ui-pagination.component.html',
    styleUrl: './ui-pagination.component.scss',
    imports: []
})
export class UiPaginationComponent {
  public routes = routes;
}
