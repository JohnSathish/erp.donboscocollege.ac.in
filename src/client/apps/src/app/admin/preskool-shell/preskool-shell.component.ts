import { Component, Renderer2, OnInit } from '@angular/core';
import { NavigationEnd, NavigationStart, Router, Event as RouterEvent, RouterModule, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../shared/preskool/header/header.component';
import { SidebarComponent } from '../shared/preskool/sidebar/sidebar.component';

// Preskool Shell - wraps dashboard with Preskool header and sidebar
@Component({
  selector: 'app-preskool-shell',
  standalone: true,
  imports: [RouterModule, RouterOutlet, CommonModule, HeaderComponent, SidebarComponent],
  templateUrl: './preskool-shell.component.html',
  styleUrl: './preskool-shell.component.scss'
})
export class PreskoolShellComponent implements OnInit {
  public miniSidebar = false;
  public expandMenu = false;
  public mobileSidebar = false;
  public themeMode: string = 'light_mode';
  public navigationColor: string = 'light_color';
  public layoutMode: string = 'default_layout';
  public fontColor: string = 'primary_font_color';
  public TopColor: string = 'white_top';
  public backgroundMode: string = '';
  public showDark = false;

  base = '';
  page = '';
  last = '';

  constructor(
    private router: Router,
    private renderer: Renderer2
  ) {
    this.router.events.subscribe((data: RouterEvent) => {
      if (data instanceof NavigationStart) {
        this.getRoutes(data);
      }
      if (data instanceof NavigationEnd) {
        localStorage.removeItem('isMobileSidebar');
        this.mobileSidebar = false;
      }
    });
    this.getRoutes(this.router);
  }

  private getRoutes(data: any): void {
    const splitVal = data.url.split('/');
    this.base = splitVal[1] || '';
    this.page = splitVal[2] || '';
    this.last = splitVal[3] || '';
  }

  ngOnInit(): void {
    // Initialize theme settings from localStorage
    this.themeMode = localStorage.getItem('themeMode') || 'light_mode';
    this.navigationColor = localStorage.getItem('navigationColor') || 'light_color';
    this.layoutMode = localStorage.getItem('layoutMode') || 'default_layout';
    this.fontColor = localStorage.getItem('fontColor') || 'primary_font_color';
    this.TopColor = localStorage.getItem('TopColor') || 'white_top';
    this.backgroundMode = localStorage.getItem('backgroundMode') || '';
    this.miniSidebar = localStorage.getItem('sideBarPosition') === 'true';
    
    // Apply theme classes
    if (this.themeMode === 'dark_mode') {
      this.renderer.addClass(document.body, 'darkmode-custom-cls');
    }
  }

  toggleSidebar(): void {
    this.miniSidebar = !this.miniSidebar;
    if (this.miniSidebar) {
      localStorage.setItem('sideBarPosition', 'true');
    } else {
      localStorage.removeItem('sideBarPosition');
    }
  }

  toggleMobileSidebar(): void {
    this.mobileSidebar = !this.mobileSidebar;
    if (this.mobileSidebar) {
      localStorage.setItem('isMobileSidebar', 'true');
    } else {
      localStorage.removeItem('isMobileSidebar');
    }
  }

  miniSideBarMouseHover(position: string): void {
    if (position === 'over' && this.miniSidebar) {
      this.expandMenu = true;
    } else {
      this.expandMenu = false;
    }
  }
}

