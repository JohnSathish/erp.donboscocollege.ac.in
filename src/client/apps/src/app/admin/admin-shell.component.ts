import { ChangeDetectionStrategy, Component, inject, signal, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ToastService } from '../shared/toast.service';
import { AdminNavigationService, NavigationItem } from './admin-navigation.service';
import { AuthService } from '../auth/auth.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-admin-shell',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, RouterModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './admin-shell.component.html',
  styleUrls: ['./admin-shell.component.scss'],
})
export class AdminShellComponent {
  protected readonly toastService = inject(ToastService);
  protected readonly navigationService = inject(AdminNavigationService);
  protected readonly router = inject(Router);
  protected readonly authService = inject(AuthService);

  protected readonly navigationItems = this.navigationService.items;
  /** Mobile overlay drawer */
  protected readonly mobileSidebarOpen = signal(false);
  protected readonly searchTerm = signal('');
  protected readonly profileDropdownOpen = signal(false);
  /** Simple notification drawer (placeholder items; wire to API later). */
  protected readonly notificationsOpen = signal(false);

  constructor() {
    this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe(() => {
      this.autoExpandActiveMenu();
      this.mobileSidebarOpen.set(false);
    });
  }

  private autoExpandActiveMenu(): void {
    const items = [...this.navigationItems()];
    items.forEach((item, index) => {
      if (item.children && this.isParentRouteActive(item)) {
        if (!item.expanded) {
          this.navigationService.toggleExpanded(index);
        }
      }
    });
  }

  toggleMobileSidebar(): void {
    this.mobileSidebarOpen.update((o) => !o);
  }

  closeMobileSidebar(): void {
    this.mobileSidebarOpen.set(false);
  }

  toggleMenu(index: number): void {
    this.navigationService.toggleExpanded(index);
  }

  onSearch(value: string): void {
    this.searchTerm.set(value);
    // Implement search functionality if needed
  }

  isRouteActive(route?: string): boolean {
    if (!route) return false;
    return this.router.url === route || this.router.url.startsWith(route + '/');
  }

  isParentRouteActive(item: NavigationItem): boolean {
    if (!item.children) return false;
    return item.children.some((child) => this.isRouteActive(child.route));
  }

  getIcon(icon: string): string {
    // Convert emoji icons to iconify icons
    const iconMap: { [key: string]: string } = {
      '🏠': 'solar:home-smile-angle-outline',
      '📊': 'solar:chart-2-outline',
      '📝': 'solar:document-text-outline',
      '✏️': 'solar:document-text-bold',
      '📋': 'solar:document-bold',
      '📄': 'solar:document-text-bold',
      '✅': 'solar:check-circle-bold',
      '🏆': 'solar:medal-ribbons-star-bold',
      '👥': 'solar:users-group-two-rounded-bold',
      '👤': 'solar:user-bold',
      '📚': 'solar:book-bookmark-bold',
      '💰': 'solar:wallet-bold',
      '📢': 'solar:megaphone-bold',
      '🏢': 'solar:buildings-bold',
      '🚌': 'solar:bus-bold',
      '📖': 'solar:book-bold',
      '⚙️': 'solar:settings-bold',
      '🎓': 'solar:diploma-bold',
      '✉️': 'solar:letter-bold',
      '✓': 'solar:check-circle-bold',
      '🏫': 'solar:buildings-2-bold',
      '⬆️': 'solar:arrow-up-bold',
      '🪪': 'solar:card-bold',
      '👨‍🏫': 'solar:user-speak-bold',
      '📑': 'solar:documents-bold',
      '⏰': 'solar:alarm-bold',
      '❓': 'solar:question-circle-bold',
      '✍️': 'solar:pen-2-bold',
      '💵': 'solar:banknote-2-bold',
      '🎫': 'solar:ticket-bold',
      '⚠️': 'solar:danger-triangle-bold',
      '📌': 'solar:pin-bold',
      '📧': 'solar:letter-unread-bold',
      '📜': 'solar:document-bold',
      '🔔': 'solar:bell-bold',
      '🚗': 'solar:car-bold',
      '🗺️': 'solar:map-bold',
      '👨‍✈️': 'solar:user-bold',
      '📥': 'solar:inbox-in-bold',
      '🛏️': 'solar:bed-bold',
      '🔐': 'solar:lock-password-bold',
    };
    return iconMap[icon] || 'solar:circle-bold';
  }

  toggleProfileDropdown(): void {
    this.profileDropdownOpen.set(!this.profileDropdownOpen());
  }

  toggleNotifications(): void {
    this.notificationsOpen.update((v) => !v);
  }

  closeNotifications(): void {
    this.notificationsOpen.set(false);
  }

  logout(): void {
    this.authService.logout('/AdminLogin');
    this.profileDropdownOpen.set(false);
  }

  getUserDisplayName(): string {
    const profile = this.authService.profile;
    if (!profile) return 'Admin';
    
    const fullName = profile.fullName || '';
    if (!fullName) return 'Admin';
    
    // Get first name and last initial (e.g., "Adrian D.")
    const parts = fullName.trim().split(' ');
    if (parts.length === 1) return parts[0];
    
    const firstName = parts[0];
    const lastInitial = parts[parts.length - 1].charAt(0).toUpperCase();
    return `${firstName} ${lastInitial}.`;
  }
}
