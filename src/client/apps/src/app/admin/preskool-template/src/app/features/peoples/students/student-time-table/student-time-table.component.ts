import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';
import { SelectModule } from 'primeng/select';
import { MatSortModule } from '@angular/material/sort';
import { FormsModule } from '@angular/forms';
import { CustomPaginationComponent } from '../../../../shared/custom-pagination/custom-pagination.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'app-student-time-table',
    templateUrl: './student-time-table.component.html',
    styleUrl: './student-time-table.component.scss',
    imports: [BsDatepickerModule,SelectModule,FormsModule,MatSortModule,RouterLink]
})
export class StudentTimeTableComponent {
  public routes = routes;
}
