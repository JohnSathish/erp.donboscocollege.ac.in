import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-financial-settings',
    templateUrl: './financial-settings.component.html',
    styleUrl: './financial-settings.component.scss',
    imports: [RouterModule,SelectModule,CommonModule]
})
export class FinancialSettingsComponent {

}
