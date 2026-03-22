import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {  RouterModule } from '@angular/router';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-system-settings',
    templateUrl: './system-settings.component.html',
    styleUrl: './system-settings.component.scss',
    imports: [RouterModule,SelectModule,CommonModule]
})
export class SystemSettingsComponent {

}
