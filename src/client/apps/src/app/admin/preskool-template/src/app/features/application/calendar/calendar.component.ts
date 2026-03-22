import { ChangeDetectorRef, Component, ViewEncapsulation } from '@angular/core';



import { CommonModule } from '@angular/common';
import { routes } from '../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';

import { RouterLink } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CustomCalendarComponent } from '../../common/custom-calendar/custom-calendar.component';

@Component({
    selector: 'app-calendar',
    templateUrl: './calendar.component.html',
    styleUrls: ['./calendar.component.scss'],
    encapsulation: ViewEncapsulation.None,
     imports: [
    CommonModule,
    SelectModule,
    RouterLink,
    BsDatepickerModule,CustomCalendarComponent
  ]
})
export class CalendarComponent {
  public routes = routes;
  country = 'India';
  calendarVisible = true;

 
  constructor(private changeDetector: ChangeDetectorRef) {}

  handleCalendarToggle() {
    this.calendarVisible = !this.calendarVisible;
  }


}
