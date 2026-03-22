import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { CustomPaginationComponent } from '../../../../shared/custom-pagination/custom-pagination.component';
import { FormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-student-result',
    templateUrl: './student-result.component.html',
    styleUrl: './student-result.component.scss',
    imports: [BsDatepickerModule,SelectModule,FormsModule,MatSortModule,RouterLink]
})
export class StudentResultComponent {
  public routes = routes;
}
