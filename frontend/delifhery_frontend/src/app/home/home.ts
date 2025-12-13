import { Component } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {TrackingService} from '../../services/tracking.service';

@Component({
  selector: 'app-home',
  standalone:true,
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
    trackingNumber = "";
    postalCode = "";

    loading = false;
    error:string | null = null;

    constructor(private trackingService: TrackingService, private router: Router) {}

    OnTrack(): void{
      this.error = null;

      const tn = this.trackingNumber.trim();
      const pc = this.postalCode.trim();

      if(!tn || !pc){
        this.error = "Please enter a Postal code and Tracking number.";
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
