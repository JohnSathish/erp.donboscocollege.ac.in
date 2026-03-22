import { Component, inject } from '@angular/core';
import { routes } from '../shared/routes/routes';
import { SidebarService } from '../shared/sidebar/sidebar.service';
import { DataService } from '../shared/data/data.service';
import { NavigationStart, Router,Event as RouterEvent, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonService } from '../shared/common/common.service';
import { menu, MenuItem, sidebarDataone, SubMenu, subMenus, url } from '../shared/model/sidebar.model';
import { CommonModule } from '@angular/common';
// import { NgScrollbarModule } from 'ngx-scrollbar'; // Optional - can be removed if not installed

@Component({
    selector: 'app-preskool-sidebar',
    standalone: true,
    templateUrl: './sidebar.component.html',
    styleUrl: './sidebar.component.scss',
    imports: [CommonModule,RouterLink,RouterLinkActive] // NgScrollbarModule removed - use native scroll if needed
})
export class SidebarComponent {
  public routes = routes;
  base = '';
  page = '';
  last = '';
  public expandMenu = false;
  currentUrl = '';

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public side_bar_data: Array<any> = [];

  private sidebar = inject(SidebarService);
  private data = inject(DataService);
  private router = inject(Router);
  private common = inject(CommonService);
  
  constructor() {
    
    this.sidebar.expandSideBar.subscribe((res: boolean) => {
      this.expandMenu = res;
    });
    this.side_bar_data = this.data.sidebarData1;
    this.common.base.subscribe((res: string) => {
      this.base = res;
    });
    this.common.page.subscribe((res: string) => {
      this.page = res;
    });
    this.common.last.subscribe((res: string) => {
      this.last = res;
    });
    this.router.events.subscribe((event: RouterEvent) => {
      if (event instanceof NavigationStart) {
        this.getRoutes(event);
        const splitVal = event.url.split('/');
        this.currentUrl = event.url;
        this.base = splitVal[1];
        this.page = splitVal[2];
   


      }
    });
    this.getRoutes(this.router);
  }

  private getRoutes(route: url): void {
    const splitVal = route.url.split('/');
    this.currentUrl = route.url;
    this.base = splitVal[1];
    this.page = splitVal[2];
    if(this.base === 'index' || this.base === 'lead-dashboard'){
      
    }
  }
 isOpen=false;
public expandSubMenusActive(): void {
  const activeMenu = sessionStorage.getItem('menuValue');
  const activePage = sessionStorage.getItem('page'); // optional, for submenu match

  if (!Array.isArray(this.side_bar_data)) {
    console.warn('Sidebar data not initialized');
    return;
  }

  this.side_bar_data.forEach((mainMenu: sidebarDataone) => {
    mainMenu.menu.forEach((resMenu: menu) => {
      // Expand only the parent matching session value
      resMenu.showSubRoute = (resMenu.menuValue === activeMenu);

      // Expand subMenus inside that menu
      resMenu.subMenus?.forEach((sub: subMenus) => {
        sub.showSubRoute = sub.base === activePage || sub.page === activePage;
      });
    });
  });
}


  public miniSideBarMouseHover(position: string): void {
    if (position == 'over') {
      this.sidebar.expandSideBar.next(true);
      // this.sidebar.sideBarPosition.next('false');
    } else {
      this.sidebar.expandSideBar.next(false);
    }
  }
  currentOpenSecondMenu: MenuItem | null = null;

  expandSubMenus(menu: MenuItem): void {
    sessionStorage.setItem('menuValue', menu.menuValue);
    this.side_bar_data.forEach((mainMenus: MenuItem) => {
      mainMenus.menu.forEach((resMenu: SubMenu) => {
        if (resMenu.menuValue === menu.menuValue) {
          menu.showSubRoute = !menu.showSubRoute;
        } else {
          resMenu.showSubRoute = false;
        }
      });
    });
    // this.side_bar_data.forEach((mainMenu: any) => {
    //   mainMenu.menu.forEach((submenu: any) => {
    //     if (submenu !== menu) {
    //       submenu.showSubRoute = false;
    //     }
    //   });
    // });
    // menu.showSubRoute = !menu.showSubRoute;

  }

  openMenuItem: MenuItem | null = null;
  openSubmenuOneItem: SubMenu[] | null = null;
  multiLevel1 = false;
  multiLevel2 = false;
  multiLevel3 = false;

  openMenu(menu: MenuItem): void {
    this.side_bar_data.forEach((mainMenu: any) => {
      if (mainMenu !== menu) {
        mainMenu.menu.forEach((submenu: subMenus) => {
          submenu.showSubRoute = false;
        });
      }
    });
    if (this.openMenuItem === menu) {
      this.openMenuItem = null;
    } else {
      this.openMenuItem = menu;
    }
  }

  openSubmenuOne(subMenus: SubMenu[]): void {
    if (this.openSubmenuOneItem === subMenus) {
      this.openSubmenuOneItem = null;
    } else {
      this.openSubmenuOneItem = subMenus;
    }
  }

  multiLevelOne() {
    this.multiLevel1 = !this.multiLevel1;
  }
  multiLevelTwo() {
    this.multiLevel2 = !this.multiLevel2;
  }
  multiLevelThree() {
    this.multiLevel3 = !this.multiLevel3;
  }
 ngOnInit(): void {
 const menuValue = sessionStorage.getItem('menuValue');

  if (!menuValue) {
    // Set to the parent menu of Deals Dashboard
    sessionStorage.setItem('menuValue', 'Dashboard');
    sessionStorage.setItem('page', 'index'); // Optional: track which submenu is open
  }

  this.expandSubMenusActive();
this.sidebar.collapseSubMenu$.subscribe(() => {
    this.collapseAllSubMenus();
  });

}
collapseAllSubMenus(): void {
  this.side_bar_data.forEach((mainMenu: sidebarDataone) => {
    mainMenu.menu.forEach((resMenu: menu) => {
      resMenu.showSubRoute = false;
    });
  });
}
}
