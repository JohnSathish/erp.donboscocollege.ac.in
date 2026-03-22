import { Component, Renderer2 } from '@angular/core';
import { SidebarService } from '../../../shared/sidebar/sidebar.service';
import { DataService } from '../../../shared/data/data.service';
import { CommonService } from '../../../shared/common/common.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { routes } from '../../../shared/routes/routes';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrl: './header.component.scss',
    imports: [CommonModule,RouterLink]
})
export class HeaderComponent {
  isNotificationVisible = false;
  public showDark = false;
  public darkTheme = false;
  toggleNotification() {
    this.isNotificationVisible = !this.isNotificationVisible;
  }
  elem = document.documentElement;
  fullscreen() {
    if (!document.fullscreenElement) {
      this.elem.requestFullscreen();
    } else {
      document.exitFullscreen();
    }
  }
  base = '';
  page = '';
  last = '';
  themeMode: string = 'light_mode';
  public miniSidebar = false;
  public routes = routes;
  constructor(private common: CommonService, private sidebar: SidebarService,private renderer:Renderer2) {
    this.common.base.subscribe((base: string) => {
      this.base = base;
    });
    this.common.page.subscribe((page: string) => {
      this.page = page;
    });
    this.common.last.subscribe((last: string) => {
      this.last = last;
    });
    this.sidebar.sideBarPosition.subscribe((res: string) => {
      if (res == 'true') {
        this.miniSidebar = true;
      } else {
        this.miniSidebar = false;
      }
    });
    this.sidebar.showDark.subscribe((res: string | boolean) => {
      if (res == 'true') {
        this.showDark = true;
        
      } else {
        this.showDark = false;
      }
    });
  }

  public toggleSidebar(): void {
    this.sidebar.switchSideMenuPosition();
  }

  public togglesMobileSideBar(): void {
    this.sidebar.switchMobileSideBarPosition();
  }

  public miniSideBarMouseHover(position: string): void {
    if (position == 'over') {
      this.sidebar.expandSideBar.next(true);
    } else {
      this.sidebar.expandSideBar.next(false);
    }
  }
  public changeTheme(theme: string): void {
    this.sidebar.changeTheme.next(theme);
    localStorage.setItem('theme', theme); 
    this.renderer.setAttribute(
      document.documentElement,
      'data-theme',
      theme
    );
  }
  public themeChange(): void {
    this.sidebar.themeColor();
  }
  
}
