import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { routes } from '../../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'app-add-staff',
    templateUrl: './add-staff.component.html',
    styleUrl: './add-staff.component.scss',
    imports: [SelectModule,FormsModule,RouterLink,BsDatepickerModule]
})
export class AddStaffComponent {
  public routes = routes;
  values: string[] = ['English', 'Spanish'];
  constructor(private router: Router) {}
  public onSubmit() {
    // this.router.navigate([routes.invoice]);
  }
}
