import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';
import { DateRangePickerComponent } from '../../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../../common/tooltip-content/tooltip-content.component';

@Component({
    selector: 'app-ticket',
    templateUrl: './ticket.component.html',
    styleUrl: './ticket.component.scss',
    imports: [SelectModule,RouterLink,DateRangePickerComponent,TooltipContentComponent]
})
export class TicketComponent { 
  public routes = routes;
}
