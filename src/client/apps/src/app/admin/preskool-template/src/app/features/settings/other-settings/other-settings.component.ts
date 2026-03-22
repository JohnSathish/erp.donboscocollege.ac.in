import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-other-settings',
    templateUrl: './other-settings.component.html',
    styleUrl: './other-settings.component.scss',
    imports: [RouterModule,SelectModule,CommonModule]
})
export class OtherSettingsComponent {

}
