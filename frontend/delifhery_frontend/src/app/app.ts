import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {Home} from './home/home';
import {Header} from './shared/header/header';
import {Footer} from './shared/footer/footer';
import {Tracking} from './tracking/tracking';
import {TrackingDetails} from './tracking-details/tracking-details';
import {ShippingCost} from './shipping-cost/shipping-cost';
import {CreateShipment} from './create-shipment/create-shipment';

@Component({
  selector: 'app-root',
  standalone:true,
  imports: [RouterOutlet, Home, Header, Footer, Tracking, ShippingCost, TrackingDetails, CreateShipment],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('delifhery_frontend');
}
