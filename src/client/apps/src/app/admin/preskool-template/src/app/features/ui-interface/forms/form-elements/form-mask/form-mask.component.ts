import { Component } from '@angular/core';
import { routes } from '../../../../../shared/routes/routes';
import { NgxMaskDirective } from 'ngx-mask';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-form-mask',
    templateUrl: './form-mask.component.html',
    styleUrls: ['./form-mask.component.scss'],
    imports: [NgxMaskDirective,CommonModule, FormsModule, ReactiveFormsModule,]
})
export class FormMaskComponent{
   routes = routes;
  
}
