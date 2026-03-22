import { Component } from '@angular/core';

import { RouterLink, RouterLinkActive, RouterModule } from '@angular/router';
import { SelectModule } from 'primeng/select';
import { CommonModule } from '@angular/common';
import { routes } from '../../../shared/routes/routes';
import { CommonService } from '../../../shared/common/common.service';

@Component({
    selector: 'app-general-settings',
    templateUrl: './general-settings.component.html',
    styleUrl: './general-settings.component.scss',
    imports: [RouterModule,SelectModule,RouterLink,RouterLinkActive,CommonModule]
})
export class GeneralSettingsComponent {
public routes = routes

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
