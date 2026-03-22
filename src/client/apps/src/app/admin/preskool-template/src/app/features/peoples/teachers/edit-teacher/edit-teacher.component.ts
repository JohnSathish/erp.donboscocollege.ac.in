import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'app-edit-teacher',
    templateUrl: './edit-teacher.component.html',
    styleUrl: './edit-teacher.component.scss',
    imports: [BsDatepickerModule,SelectModule,FormsModule,RouterLink]
})
export class EditTeacherComponent {
  public routes = routes;
  values: string[] = ['English', 'Spanish'];
}
