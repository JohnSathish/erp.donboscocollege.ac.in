import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { NavigationStart, Router, Event as RouterEvent, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
   title = 'Preskool Admin Template';
  
}
