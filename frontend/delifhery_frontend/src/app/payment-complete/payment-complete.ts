import { Component } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {ActivatedRoute} from '@angular/router';
import {DecimalPipe} from '@angular/common';

interface PaymentSummary {
  paymentId: string;
  status: string;
  amount: number;
  currency: string;
  trackingNumber: string;
  recipientName: string;
  recipientStreet: string;
  recipientPostalCode: string;
  recipientCity: string;
  labelImage: string;
}

@Component({
  selector: 'app-payment-complete',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './payment-complete.html',
  styleUrl: './payment-complete.css',
})
export class PaymentComplete {
    paymentId: string  | null = null;
    status: string | null = null;
    isLoading = true;

    errorMessage: string | null = null;
    summary: PaymentSummary | null = null;

    private readonly apiBaseUrl = environment.apiBaseUrl;

    constructor(private http: HttpClient, private route: ActivatedRoute) {
      this.route.queryParamMap.subscribe(params => {
        this.paymentId = params.get('payment-id');
        this.status = params.get('status');

        if (this.paymentId) {
          this.loadSummary(this.paymentId);
        } else {
          this.isLoading = false;
          this.errorMessage = 'Missing payment-id in URL.';
        }
      });
    }

    private loadSummary(paymentId: string): void {
      this.isLoading = true;
      this.errorMessage = null;
      this.summary = null;

      this.http.get<PaymentSummary>(`${this.apiBaseUrl}/api/payments/summary`, {params: {paymentId}})
        .subscribe({
          next: (res) => {
            this.summary = res;
            this.isLoading = false;
          },
          error: (err) => {
            console.error(err);
            this.errorMessage = err?.error?.message ?? 'Failed to load shipping cost';
            this.isLoading = false;
          },
        });
    }

    get labelDataUrl(): string | null {
      if(!this.summary?.labelImage) {
        return null;
      }
      return `data:image/png;base64,${this.summary.labelImage}`;
    }

    copyTracking(): void {
      if(!this.summary?.trackingNumber) {
        return;
      }
      navigator.clipboard.writeText(this.summary.trackingNumber)
        .catch((err) => {console.error('Clipboard failed', err)});
    }
}
