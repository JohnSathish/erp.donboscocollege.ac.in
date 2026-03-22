import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { routes } from '../../../shared/routes/routes';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-two-step-verification-3',
    templateUrl: './two-step-verification-3.component.html',
    styleUrl: './two-step-verification-3.component.scss',
   imports: [CommonModule,FormsModule]
})
export class TwoStepVerification3Component {
  public routes = routes;
  constructor(private router: Router) {}

  public oneTimePassword = {
    data1: "",
    data2: "",
    data3: "",
    data4: ""
  };
  public ValueChanged(data: string, box: string): void {
    if (box == 'digit-1' && data.length > 0) {
      document.getElementById('digit-2')?.focus();
    } else if (box == 'digit-2' && data.length > 0) {
      document.getElementById('digit-3')?.focus();
    } else if (box == 'digit-3' && data.length > 0) {
      document.getElementById('digit-4')?.focus();
    } else {
      return
    }
  }
  public tiggerBackspace(data: string, box: string) {
    let StringyfyData;
    if (data) {
      StringyfyData = data.toString();
    } else {
      StringyfyData = null;
    }
    if (box == 'digit-4' && StringyfyData == null) {
      document.getElementById('digit-3')?.focus();
    } else if (box == 'digit-3' && StringyfyData == null) {
      document.getElementById('digit-2')?.focus();
    } else if (box == 'digit-2' && StringyfyData == null) {
      document.getElementById('digit-1')?.focus();
    }
  }
  navigation() {
    this.router.navigate([routes.adminDashboard])
  }
}
