import {
  APP_INITIALIZER,
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {authInterceptor} from './auth/auth.interceptor';
import {KeycloakService} from 'keycloak-angular';
import {initKeycloak} from './auth/keycloak-init.factory';
import {CustomerInitService} from './auth/customer-init.service';
import {initCustomer} from './auth/customer-init.factory';
import {appInitializer} from './auth/app-init.factory';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    KeycloakService,
    CustomerInitService,
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializer,
      deps:[KeycloakService,CustomerInitService],
      multi: true,
    }

  ]
};
