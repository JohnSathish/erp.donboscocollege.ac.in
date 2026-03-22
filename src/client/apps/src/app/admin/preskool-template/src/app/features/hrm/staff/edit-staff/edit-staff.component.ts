import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { Router, RouterLink } from '@angular/router';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'app-edit-staff',
    templateUrl: './edit-staff.component.html',
    styleUrl: './edit-staff.component.scss',
    imports: [SelectModule,FormsModule,RouterLink,BsDatepickerModule]
})
export class EditStaffComponent {
  public routes = routes;
  values: string[] = ['English', 'Spanish'];
  constructor(private router: Router) {}
  public onSubmit() {
    // this.router.navigate([routes.invoice]);
  }
}
