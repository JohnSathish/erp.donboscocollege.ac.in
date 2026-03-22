import { Component, Renderer2, ViewChild } from '@angular/core';

import { routes } from '../../../shared/routes/routes';
import { CommonService } from '../../../shared/common/common.service';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { DateRangePickerComponent } from '../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../common/tooltip-content/tooltip-content.component';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DatePickerModule } from 'primeng/datepicker';
import { FormsModule } from '@angular/forms';
import { CustomCalendarComponent } from '../../common/custom-calendar/custom-calendar.component';
@Component({
    selector: 'app-events',
    templateUrl: './events.component.html',
    styleUrl: './events.component.scss',
    imports: [BsDatepickerModule,SelectModule,RouterLink,CommonModule,DatePickerModule,FormsModule,CustomCalendarComponent]
})
export class EventsComponent {
  public base = '';
  public page = '';
  public last = '';
  public routes = routes;

  addtime2: Date | undefined;
  showAddEventModal = false;
  showEventDetailsModal = false;
  eventDetails = { title: '' };


  handleDateClick() {
    this.showAddEventModal = true;
  }



  handleAddEventClose() {
    this.showAddEventModal = false;
  }

  handleEventDetailsClose() {
    this.showEventDetailsModal = false;
  }

 
}
