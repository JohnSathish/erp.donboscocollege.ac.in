import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { CommonService } from '../common/common.service';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {

    private renderer: Renderer2;
  base = '';
  page = '';
  last = '';
  // Theme Mode
  public themeMode: BehaviorSubject<string> = new BehaviorSubject<string>(
    localStorage.getItem('themeMode') || 'light_mode'
  );

  // Layout Mode
  public layoutMode: BehaviorSubject<string> = new BehaviorSubject<string>(
    localStorage.getItem('layoutMode') || ''
  );

  // Navigation Color
  public navigationColor: BehaviorSubject<string> = new BehaviorSubject<string>(
    localStorage.getItem('navigationColor') || 'light_mode'
  );

  // Font Color
  public fontColor: BehaviorSubject<string> = new BehaviorSubject<string>(
    localStorage.getItem('fontColor') || 'primary_font_color'
  );

   // top Color
   public TopColor: BehaviorSubject<string> = new BehaviorSubject<string>(
    localStorage.getItem('TopColor') || ''
  );

   // Font Color
   public backgroundMode: BehaviorSubject<string> = new BehaviorSubject<string>(
    localStorage.getItem('backgroundMode') || ''
  );
  constructor(rendererFactory: RendererFactory2,private common: CommonService) {
    this.renderer = rendererFactory.createRenderer(null, null);
    this.common.base.subscribe((res: string) => {
      this.base = res;
    });
    this.common.page.subscribe((res: string) => {
      this.page = res;
    });
    this.common.last.subscribe((res: string) => {
      this.last = res;
    });
  }

  public changeThemeMode(theme: string): void {
    this.themeMode.next(theme);
    localStorage.setItem('themeMode', theme);
  }
  public changeLayoutMode(layout: string): void {
    this.layoutMode.next(layout);
    localStorage.setItem('layoutMode', layout);
    this.renderer.addClass(
      document.body,'layout-mode-rtl'
    )
    
   
  }
  public changeNavigationColor(color: string): void {
    this.navigationColor.next(color);
    localStorage.setItem('navigationColor', color);
    this.renderer.setAttribute(
      document.documentElement,
      'data-sidebar', color 
    );
  }
  public changeFontColor(color: string): void {
    this.fontColor.next(color);
    localStorage.setItem('fontColor', color);
     this.renderer.setAttribute(
      document.documentElement,
      'data-color', color 
    );
  }
  public changeTopColor(top: string): void {
    this.TopColor.next(top);
    localStorage.setItem('TopColor', top);
   this.renderer.setAttribute(
      document.documentElement,
      'data-topbar', top 
    );
  }

  public changeBackgroundMode(background: string): void {
    this.backgroundMode.next(background);
    localStorage.setItem('backgroundMode', background); 
    this.renderer.setAttribute(
      document.body,
      'data-sidebarbg', background 
    );
  }
}
