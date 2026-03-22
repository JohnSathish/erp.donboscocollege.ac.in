import { Component, Renderer2 } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { CustomPaginationComponent } from '../../../../shared/custom-pagination/custom-pagination.component';
import { FormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { RouterLink } from '@angular/router';
import { DateRangePickerComponent } from '../../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../../common/tooltip-content/tooltip-content.component';

@Component({
    selector: 'app-student-grid',
    templateUrl: './student-grid.component.html',
    styleUrl: './student-grid.component.scss',
   imports: [BsDatepickerModule,SelectModule,FormsModule,MatSortModule,RouterLink,DateRangePickerComponent,TooltipContentComponent]
})
export class StudentGridComponent {
  public routes = routes;
  constructor(
    private renderer: Renderer2,

  ) {


    
  }
  public handleApplyClick = () => {
    const modalElements = document.getElementsByClassName('drop-width');
    if (modalElements.length > 0) {
      for (let i = 0; i < modalElements.length; i++) {
        const modalElement = modalElements[i];
        this.renderer.removeClass(modalElement, 'show');
      }
    }
  };
}
