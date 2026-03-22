import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-storage',
    templateUrl: './storage.component.html',
    styleUrl: './storage.component.scss',
    imports: [RouterLink]
})
export class StorageComponent {
  public routes = routes;
}
