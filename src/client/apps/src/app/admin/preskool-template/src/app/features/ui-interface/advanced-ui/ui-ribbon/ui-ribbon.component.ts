import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-ribbon',
    templateUrl: './ui-ribbon.component.html',
    styleUrl: './ui-ribbon.component.scss',
    imports: []
})
export class UiRibbonComponent {
  public routes = routes;
}
