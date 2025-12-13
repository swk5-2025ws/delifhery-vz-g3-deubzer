import { Component } from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {TrackingService, TrackingStatusResponse} from '../../services/tracking.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-tracking',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tracking.html',
  styleUrl: './tracking.css',
})
export class Tracking {
  trackingNumber= "";
  postalCode = "";

  loading = false;
  error : string | null = null;

  data: TrackingStatusResponse | null = null;

  constructor(private trackingService: TrackingService, private router: Router) {}

  OnTrack(): void {
    this.error = null;
    this.data = null;

    const tn = this.trackingNumber.trim();
    const pc = this.postalCode.trim();

    if(!tn || !pc){
      this.error = "Please enter a Postal code and Tracking number.";
      return;
    }

    this.loading = true;

    this.trackingService.trackShipment({trackingNumber: tn, postalCode: pc}).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/tracking',pc], {queryParams: { tn }});
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.status === 404 ? "No package found" : "Loading error. Try again.";

      },
    });
  }

}
