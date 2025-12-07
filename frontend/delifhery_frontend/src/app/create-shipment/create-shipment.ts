import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {ShipmentRequest, ShipmentResponse, ShipmentService} from '../../services/shipment.service';

@Component({
  selector: 'app-create-shipment',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
  ],
  templateUrl: './create-shipment.html',
  styleUrl: './create-shipment.css',
})
export class CreateShipment {
  form: FormGroup;
  isLoading = false;
  errorMessage: string | null = null;
  result: ShipmentResponse | null = null;

  constructor(
    private fb: FormBuilder,
    private shipmentService: ShipmentService
  ) {
    this.form = this.fb.group({
      senderName: ['', Validators.required],
      senderStreet: ['', Validators.required],
      senderPostalCode: ['', [Validators.required, Validators.minLength(4)]],
      senderCity: ['', Validators.required],
      saveSenderAsDefault: [false],
      recipientName: ['', Validators.required],
      recipientStreet: ['', Validators.required],
      recipientPostalCode: ['', [Validators.required, Validators.minLength(4)]],
      recipientCity: ['', Validators.required],
      lengthCm: [null as number | null, [Validators.required, Validators.min(1)]],
      widthCm: [null as number | null, [Validators.required, Validators.min(1)]],
      heightCm: [null as number | null, [Validators.required, Validators.min(1)]],
      weightKg: [
        null as number | null,
        [Validators.required, Validators.min(0.1)],
      ],
    });
  }

  onSubmit(): void {
    this.errorMessage = null;
    this.result = null;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    const value = this.form.value;

    const request: ShipmentRequest = {
      SenderName: value.senderName,
      SenderStreet: value.senderStreet,
      SenderPostalCode: value.senderPostalCode,
      SenderCity: value.senderCity,

      RecipientName: value.recipientName,
      RecipientStreet: value.recipientStreet,
      RecipientPostalCode: value.recipientPostalCode,
      RecipientCity: value.recipientCity,

      LengthCm: value.lengthCm,
      WidthCm: value.widthCm,
      HeightCm: value.heightCm,
      WeightKg: value.weightKg,
    };

    this.shipmentService.createShipment(request).subscribe({
      next: (res) => {
        this.result = res;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage =
          err?.error?.message ?? 'Failed to create shipment';
        this.isLoading = false;
      },
    });
  }

  onReset(): void {
    this.form.reset({
      saveSenderAsDefault: false,
    });
    this.result = null;
    this.errorMessage = null;
  }

  goToPayment(): void {
    if (this.result?.paymentUrl) {
      window.location.href = this.result.paymentUrl;
    }
  }

  isInvalid(controlName: string): boolean {
    const c = this.form.get(controlName);
    return !!(c && c.invalid && (c.touched || c.dirty));
  }

}
