import { Routes } from '@angular/router';
import {Home} from './home/home';
import {Tracking} from './tracking/tracking';
import {TrackingDetails} from './tracking-details/tracking-details';
import {ShippingCost} from './shipping-cost/shipping-cost';
import {MyContact} from './my-contact/my-contact';
import {CreateShipment} from './create-shipment/create-shipment';
import {PaymentComplete} from './payment-complete/payment-complete';

export const routes: Routes = [
  {
    path:'',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: Home
  },
  {
    path: 'tracking',
    component: Tracking
  },
  {
    path: 'tracking/:trackingId',
    component: TrackingDetails
  },
  {
    path: 'shippingCost',
    component: ShippingCost
  },
  {
    path: 'myContacts',
    component: MyContact
  },
  {
    path: 'shipments',
    component: CreateShipment
  },
  {
    path: 'payment-complete',
    loadComponent: () => import('./payment-complete/payment-complete'). then(m => m.PaymentComplete),
  }

];
