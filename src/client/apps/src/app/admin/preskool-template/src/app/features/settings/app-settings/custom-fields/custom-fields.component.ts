import { Component } from '@angular/core';

import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';
import { routes } from '../../../../shared/routes/routes';

@Component({
    selector: 'app-custom-fields',
    templateUrl: './custom-fields.component.html',
    styleUrl: './custom-fields.component.scss',
    imports: [SelectModule,RouterLink]
})
export class CustomFieldsComponent {
  public routes = routes;
}
