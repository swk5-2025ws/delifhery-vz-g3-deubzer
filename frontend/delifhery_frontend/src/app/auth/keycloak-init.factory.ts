import { KeycloakService} from 'keycloak-angular';
import { environment} from '../../environments/environment';

export function initKeycloak(keycloak: KeycloakService) {
  return () =>keycloak.init({
    config: {
      url: environment.keycloak.url,
      realm: environment.keycloak.realm,
      clientId: environment.keycloak.clientId,
    },
    initOptions: {
      onLoad: 'check-sso',
      checkLoginIframe: false
    },
    bearerExcludedUrls: ['/assets']
  })
}
