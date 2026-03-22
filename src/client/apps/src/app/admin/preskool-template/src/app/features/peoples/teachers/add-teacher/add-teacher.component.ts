import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
interface Option {
  label: string;
  value: string;
}
@Component({
    selector: 'app-add-teacher',
    templateUrl: './add-teacher.component.html',
    styleUrl: './add-teacher.component.scss',
    imports: [BsDatepickerModule,SelectModule,FormsModule,RouterLink]
})
export class AddTeacherComponent {
  public routes = routes;
  values: string[] = ['English', 'Spanish'];
}
