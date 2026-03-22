import { Component } from '@angular/core';
import { routes } from '../../../../../shared/routes/routes';


@Component({
    selector: 'app-form-checkbox-radios',
    templateUrl: './form-checkbox-radios.component.html',
    styleUrl: './form-checkbox-radios.component.scss',
    imports: []
})
export class FormCheckboxRadiosComponent {
  public routes = routes;
}
