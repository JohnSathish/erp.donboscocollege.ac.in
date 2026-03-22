import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-localization',
    templateUrl: './localization.component.html',
    styleUrl: './localization.component.scss',
    imports: [SelectModule,FormsModule,]
})
export class LocalizationComponent {
  values: string[] = ['JPG', 'Spanish','PNG'];
}
