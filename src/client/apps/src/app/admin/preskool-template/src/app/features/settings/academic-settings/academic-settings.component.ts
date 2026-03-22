import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CommonService } from '../../../shared/common/common.service';
import { routes } from '../../../shared/routes/routes';

@Component({
    selector: 'app-academic-settings',
    templateUrl: './academic-settings.component.html',
    styleUrl: './academic-settings.component.scss',
    imports: [RouterModule,CommonModule]
})
export class AcademicSettingsComponent {
  public routes = routes;
  base = '';
  page = '';
  last = '';
  constructor(
    private common: CommonService
  ) {

    this.common.base.subscribe((res: string) => {
      this.base = res;
    });
    this.common.page.subscribe((res: string) => {
      this.page = res;
    });
    this.common.page.subscribe((res: string) => {
      this.last = res;
    });
    
  }
}
