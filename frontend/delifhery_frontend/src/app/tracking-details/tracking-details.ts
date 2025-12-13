import {Component, OnInit} from '@angular/core';
import {TrackingService, TrackingStatusResponse} from '../../services/tracking.service';
import {CommonModule} from '@angular/common';
import {ActivatedRoute, RouterLink} from '@angular/router';

@Component({
  selector: 'app-tracking-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './tracking-details.html',
  styleUrl: './tracking-details.css',
})
export class TrackingDetails implements OnInit{
  loading = false;
  error: string | null = null;

  trackingNumber= "";
  postalCode = "";

  data: TrackingStatusResponse | null = null;
  constructor(private trackingService: TrackingService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.postalCode = (this.route.snapshot.paramMap.get('postalCode') ?? '').trim();
    this.trackingNumber = (this.route.snapshot.queryParamMap.get('tn') ?? '').trim();

    if(!this.postalCode || !this.trackingNumber){
      this.error = "Missing tracking number or postal code. Try again later";
      return;
    }

    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = null;
    this.data = null;

    this.trackingService.trackShipment({
      trackingNumber: this.trackingNumber,
      postalCode: this.postalCode,
    }).subscribe({
      next:(res) => {
        this.data = res;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.status === 404 ? "No package found." : "Loading error";
      }
    });
  }
  get currentStatus(): string {
    if(!this.data?.history?.length){
      return "-";
    }
    return this.data.history[this.data.history.length - 1].status ?? "-";
  }

  get lastUpdate(): string {
    if(!this.data?.history?.length){
      return "-";
    }
    const last = this.data.history[this.data.history.length - 1].occurredAt;
    return last ? new Intl.DateTimeFormat("de-At",{ dateStyle: 'medium', timeStyle: 'short' }).format(last) : "-";
  }
}
