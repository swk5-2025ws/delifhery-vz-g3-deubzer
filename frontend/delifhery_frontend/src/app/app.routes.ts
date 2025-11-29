import { Routes } from '@angular/router';
import {Home} from './home/home';
import {Tracking} from './tracking/tracking';

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
  }
];
