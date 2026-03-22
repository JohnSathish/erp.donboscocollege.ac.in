import { Component, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplicantPortalStore } from '../dashboard/applicant-portal.store';
import { ApplicantNotificationReadService } from '../dashboard/applicant-notification-read.service';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss'],
})
export class NotificationsComponent implements OnInit {
  private readonly store = inject(ApplicantPortalStore);
  private readonly notificationRead = inject(ApplicantNotificationReadService);

  readonly notifications = computed(
    () => this.store.dashboard()?.notifications ?? []
  );

  async ngOnInit(): Promise<void> {
    if (!this.store.dashboard()) {
      await this.store.loadDashboard();
    }
    const id = this.store.dashboard()?.profile?.uniqueId;
    if (id) {
      this.notificationRead.markNotificationsPageSeen(id);
    }
  }
}

