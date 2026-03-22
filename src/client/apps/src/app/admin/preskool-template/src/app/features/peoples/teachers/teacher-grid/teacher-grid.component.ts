import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DateRangePickerComponent } from '../../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../../common/tooltip-content/tooltip-content.component';

@Component({
    selector: 'app-teacher-grid',
    templateUrl: './teacher-grid.component.html',
    styleUrl: './teacher-grid.component.scss',
    imports: [BsDatepickerModule,SelectModule,FormsModule,RouterLink,DateRangePickerComponent,TooltipContentComponent]
})
export class TeacherGridComponent {
  public routes = routes;
}
