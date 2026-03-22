import { Component, EventEmitter, Output, Renderer2 } from '@angular/core';
import { SettingsService } from '../../../shared/settings/settings.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Event as RouterEvent } from '@angular/router';
import { NavigationStart } from '@angular/router';
import { NavigationEnd } from '@angular/router';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrl: './layout.component.scss',
    imports: [CommonModule]
})
export class LayoutComponent {
  showPreview: boolean = false;
  themeMode: string = 'light_mode';
  layoutMode: string = 'light_mode';
  navigationColor: string = 'light_mode';
  fontColor: string = 'primary_font_color';
  TopColor: string = 'white_top';
  backgroundMode:string ="";
  rtl=false;

  @Output() previewToggled: EventEmitter<boolean> = new EventEmitter<boolean>();
  togglePreview(): void {
    this.showPreview = !this.showPreview;
    this.previewToggled.emit(this.showPreview);
  }
  constructor(public settings: SettingsService, private router: Router,private renderer:Renderer2) {
    this.settings.themeMode.subscribe((res: string) => {
      this.themeMode = res;
    });
    this.settings.layoutMode.subscribe((res: string) => {
      this.layoutMode = res;
    });
    this.settings.navigationColor.subscribe((res: string) => {
      this.navigationColor = res;
    });
    this.settings.fontColor.subscribe((res: string) => {
      this.fontColor = res;
    });
    this.settings.TopColor.subscribe((res: string) => {
      this.TopColor = res;
    });
    this.settings.backgroundMode.subscribe((res: string) => {
      this.backgroundMode = res;
    });
    this.router.events.subscribe((data: RouterEvent) => {
      if (data instanceof NavigationStart) {
        
      }
      if (data instanceof NavigationEnd) {
        this.rtl=false;
      }
    });
  }

  public changeNavigationColor(color: string): void {
    this.settings.navigationColor.next(color);
    localStorage.setItem('navigationColor', color);
  }
  public changeFontColor(color: string): void {
    this.settings.fontColor.next(color);
    localStorage.setItem('fontColor', color);
  }
  public changeThemeMode(theme: string): void {
    this.settings.themeMode.next(theme);
    localStorage.setItem('themeMode', theme);
  }
  public changeLayoutMode(layout: string): void {
    this.settings.layoutMode.next(layout);
    localStorage.setItem('layoutMode', layout);
    this.renderer.removeClass(
      document.body,'layout-mode-rtl'
    )
    
  }
  public layoutrtl(rtl:string):void{
    this.settings.layoutMode.next(rtl);
    this.renderer.addClass(
      document.body,rtl
    )
    this.rtl=true
  }
  public changeTopColor(top: string): void {
    this.settings.TopColor.next(top);
    localStorage.setItem('TopColor', top);
    
  }
  public changeBackgroundMode(background: string): void {
    this.settings.backgroundMode.next(background);
    localStorage.setItem('backgroundMode', background);
  }

  resetAllMode() {
    this.settings.changeThemeMode('light_mode');
    this.settings.changeNavigationColor('light_color');
    this.settings.changeFontColor('primary_font_color');
    this.settings.changeLayoutMode('default_mode');
    this.settings.changeTopColor('white_top');
    this.settings.changeBackgroundMode('');
    this.settings.changeLayoutMode('default_layout');
    this.rtl=false;
  }
}
