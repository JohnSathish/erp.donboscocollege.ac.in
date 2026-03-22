import { Route } from '@angular/router';
import { RegistrationComponent } from './registration/registration.component';
import { OfflineAdmissionComponent } from './offline-admission/offline-admission.component';

export const appRoutes: Route[] = [
  { path: '', pathMatch: 'full', redirectTo: 'register' },
  { path: 'register', component: RegistrationComponent },
  { path: 'offline-admission', component: OfflineAdmissionComponent },
  { path: '**', redirectTo: 'register' },
];
