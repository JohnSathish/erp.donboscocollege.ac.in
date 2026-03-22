import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { routes } from '../../../../shared/routes/routes';

@Component({
    selector: 'app-prefixes',
    templateUrl: './prefixes.component.html',
    styleUrl: './prefixes.component.scss',
    imports: []
})
export class PrefixesComponent {
  public routes = routes;
  constructor(private router: Router) {}
  public onSubmit() {
    // this.router.navigate([routes.invoice]);
  }
}
