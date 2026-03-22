import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterModule } from '@angular/router';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-app-settings',
    templateUrl: './app-settings.component.html',
    styleUrl: './app-settings.component.scss',
    imports: [RouterModule,SelectModule,CommonModule]
})
export class AppSettingsComponent {

}
