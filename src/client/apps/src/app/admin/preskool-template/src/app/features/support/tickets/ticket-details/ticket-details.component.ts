import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';
import { DateRangePickerComponent } from '../../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../../common/tooltip-content/tooltip-content.component';

@Component({
    selector: 'app-ticket-details',
    templateUrl: './ticket-details.component.html',
    styleUrl: './ticket-details.component.scss',
    imports: [SelectModule,RouterLink,TooltipContentComponent]
})
export class TicketDetailsComponent {
  public routes = routes;

}
