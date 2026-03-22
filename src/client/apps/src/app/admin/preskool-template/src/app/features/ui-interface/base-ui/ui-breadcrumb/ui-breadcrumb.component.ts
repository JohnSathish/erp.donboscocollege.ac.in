import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-breadcrumb',
    templateUrl: './ui-breadcrumb.component.html',
    styleUrl: './ui-breadcrumb.component.scss',
    imports: []
})
export class UiBreadcrumbComponent {
  public routes = routes;
}
