import { Component } from '@angular/core';
import { routes } from '../../../../../shared/routes/routes';


@Component({
    selector: 'app-form-floating-labels',
    templateUrl: './form-floating-labels.component.html',
    styleUrl: './form-floating-labels.component.scss',
    imports: []
})
export class FormFloatingLabelsComponent {
  public routes = routes;
}
