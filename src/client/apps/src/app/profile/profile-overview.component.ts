import { Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplicantPortalStore } from '../dashboard/applicant-portal.store';

@Component({
  selector: 'app-profile-overview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile-overview.component.html',
  styleUrls: ['./profile-overview.component.scss'],
})
export class ProfileOverviewComponent {
  private readonly store = inject(ApplicantPortalStore);

  readonly profile = computed(() => this.store.dashboard()?.profile ?? null);
}
