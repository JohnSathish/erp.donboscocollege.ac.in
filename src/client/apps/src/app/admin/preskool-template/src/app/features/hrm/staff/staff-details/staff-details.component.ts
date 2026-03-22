import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-staff-details',
    templateUrl: './staff-details.component.html',
    styleUrl: './staff-details.component.scss',
    imports: [RouterLink]
})
export class StaffDetailsComponent {
  public routes = routes;
}
