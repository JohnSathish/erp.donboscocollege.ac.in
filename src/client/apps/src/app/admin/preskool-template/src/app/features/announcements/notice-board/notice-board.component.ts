import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { DateRangePickerComponent } from '../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../common/tooltip-content/tooltip-content.component';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-notice-board',
    templateUrl: './notice-board.component.html',
    styleUrl: './notice-board.component.scss',
    imports: [BsDatepickerModule,SelectModule,DateRangePickerComponent,TooltipContentComponent,RouterLink,CommonModule]
})
export class NoticeBoardComponent {
  public routes = routes;
}
