import { Component } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { CustomPaginationComponent } from '../../../shared/custom-pagination/custom-pagination.component';
import { FormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { DateRangePickerComponent } from '../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../common/tooltip-content/tooltip-content.component';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-membership-addons',
    templateUrl: './membership-addons.component.html',
    styleUrl: './membership-addons.component.scss',
    imports: [FormsModule,MatSortModule,RouterLink,CommonModule,SelectModule]
})
export class MembershipAddonsComponent {
  public routes = routes;
}
