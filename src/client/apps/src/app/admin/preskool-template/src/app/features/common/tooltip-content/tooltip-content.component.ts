import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
@Component({
    selector: 'app-tooltip-content',
    templateUrl: './tooltip-content.component.html',
    styleUrl: './tooltip-content.component.scss',
  imports: [CommonModule,TooltipModule],
})
export class TooltipContentComponent {

}
