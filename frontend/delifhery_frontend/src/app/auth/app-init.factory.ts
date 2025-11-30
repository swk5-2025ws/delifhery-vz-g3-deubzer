import { KeycloakService } from 'keycloak-angular';
import { environment } from '../../environments/environment';
import { CustomerInitService } from './customer-init.service';
import { firstValueFrom } from 'rxjs';

export function appInitializer(
  keycloak: KeycloakService,
  customerInit: CustomerInitService
) {
  return async () => {
    await keycloak.init({
      config: {
        url: environment.keycloak.url,
        realm: environment.keycloak.realm,
        clientId: environment.keycloak.clientId,
      },
      initOptions: {
        onLoad: 'check-sso',
        checkLoginIframe: false,
      },
      bearerExcludedUrls: ['/assets'],
    });

    const loggedIn = await keycloak.isLoggedIn();

    if (loggedIn) {
      try {
        await firstValueFrom(customerInit.ensureCurrentCustomer());
      } catch (err) {
        console.error('Error ensuring customer in DB', err);
      }
    }
  };
}
