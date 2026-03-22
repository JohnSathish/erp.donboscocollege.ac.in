import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'app-teacher-details',
    templateUrl: './teacher-details.component.html',
    styleUrl: './teacher-details.component.scss',
    imports: [BsDatepickerModule,SelectModule,FormsModule,RouterLink]
})
export class TeacherDetailsComponent {
  public routes = routes;
}
