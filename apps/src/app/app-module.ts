import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { App } from './app';
import { appRoutes } from './app.routes';
import { RegistrationComponent } from './registration/registration.component';
import { API_BASE_URL } from '@client/shared/util';
import { environment } from '../environments/environment';

@NgModule({
  declarations: [App, RegistrationComponent, OfflineAdmissionComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes),
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    { provide: API_BASE_URL, useValue: environment.apiBaseUrl },
  ],
  bootstrap: [App],
})
export class AppModule {}
