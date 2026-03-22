import { Injectable, signal } from '@angular/core';
import { ApplicantNotificationDto } from '@client/shared/data';

/**
 * Persists when the applicant last opened the Notifications page so the sidebar
 * badge can show unread count instead of total list length (API has no read state).
 */
@Injectable({ providedIn: 'root' })
export class ApplicantNotificationReadService {
  private readonly bump = signal(0);

  /** Depend on this in computed() so badge updates after markSeen(). */
  readonly readEpoch = this.bump.asReadonly();

  private key(uniqueId: string): string {
    return `erp.applicant.notifications.lastSeenUtc.${uniqueId}`;
  }

  getLastSeenUtc(uniqueId: string): Date | null {
    try {
      const raw = localStorage.getItem(this.key(uniqueId));
      if (!raw) {
        return null;
      }
      const d = new Date(raw);
      return Number.isNaN(d.getTime()) ? null : d;
    } catch {
      return null;
    }
  }

  /** Call when the user opens /app/notifications. */
  markNotificationsPageSeen(uniqueId: string): void {
    localStorage.setItem(this.key(uniqueId), new Date().toISOString());
    this.bump.update((n) => n + 1);
  }

  unreadCount(
    notifications: ApplicantNotificationDto[] | undefined | null,
    uniqueId: string | undefined | null
  ): number {
    this.bump();
    const list = notifications ?? [];
    if (!list.length || !uniqueId) {
      return 0;
    }
    const lastSeen = this.getLastSeenUtc(uniqueId);
    if (!lastSeen) {
      return list.length;
    }
    return list.filter((n) => new Date(n.createdOnUtc) > lastSeen).length;
  }
}
