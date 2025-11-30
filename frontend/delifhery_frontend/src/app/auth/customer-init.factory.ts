import { KeycloakService } from 'keycloak-angular';
import { CustomerInitService } from './customer-init.service';
import { firstValueFrom } from 'rxjs';

export function initCustomer(
  keycloak: KeycloakService,
  customerInit: CustomerInitService
) {
  return async () => {
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
