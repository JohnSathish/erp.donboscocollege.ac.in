import { Component, Renderer2 } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import {  MultiSelectModule } from 'primeng/multiselect';
import { DateRangePickerComponent } from '../../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../../common/tooltip-content/tooltip-content.component';
interface City {
  name: string,
  code: string
}
interface City2 {
  name: string,
  code: string
}
@Component({
    selector: 'app-guardian-grid',
    templateUrl: './guardian-grid.component.html',
    styleUrl: './guardian-grid.component.scss',
    imports: [BsDatepickerModule,SelectModule,FormsModule,RouterLink,DateRangePickerComponent,TooltipContentComponent,MultiSelectModule]
})
export class GuardianGridComponent {
  public routes = routes;
  cities!: City[];
  cities2!: City2[];

  selectedCities1!: City[];
  selectedCities2!: City2[];

  constructor(private renderer: Renderer2, private router: Router) {
      this.cities = [
          {name: 'Janet', code: 'Janet'},
          {name: 'Joann', code: 'Joann'},
          {name: 'Kathleen', code: 'Kathleen'},
          {name: 'Gifford', code: 'Gifford'},
         
      ];
      this.cities = [
        {name: 'Janet', code: 'Janet'},
        {name: 'Joann', code: 'Joann'},
        {name: 'Kathleen', code: 'Kathleen'},
        {name: 'Gifford', code: 'Gifford'},
    ];
  }
  navigateToGigs() {
    // Remove the modal backdrop
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) {
      this.renderer.removeChild(backdrop.parentNode, backdrop);
    }

    // Navigate to the specified route
    this.router.navigate(['peoples/students/student-details']);
  }
}
