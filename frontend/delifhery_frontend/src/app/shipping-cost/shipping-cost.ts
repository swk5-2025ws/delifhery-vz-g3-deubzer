import { Component } from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {ShippingCostService, ShippingPriceResponse} from '../../services/shipping-cost.service';

@Component({
  selector: 'app-shipping-cost',
  standalone:true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './shipping-cost.html',
  styleUrl: './shipping-cost.css',
})
export class ShippingCost {
  isLoading = false;
  errorMessage: string | null = null;
  result: ShippingPriceResponse | null = null;
  form!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private _shippingCostService: ShippingCostService,
  ) {
    this.form = this.formBuilder.group({
      senderPostalCode: ['', [Validators.required, Validators.minLength(4)]],
      senderCity: ['', Validators.required],
      senderStreet: ['', Validators.required],
      recipientPostalCode: ['', [Validators.required, Validators.minLength(4)]],
      recipientCity: ['', Validators.required],
      recipientStreet: ['', Validators.required],

      widthCm: [null as number | null, [Validators.required, Validators.min(1)]],
      heightCm: [null as number | null, [Validators.required, Validators.min(1)]],
      lengthCm: [null as number | null, [Validators.required, Validators.min(1)]],
      weightKg: [null as number | null, [Validators.required, Validators.min(0.1)]],
    });
  }

  isInvalid(controlName: string): boolean {
    const c = this.form.get(controlName);
    return !!(c && c.invalid && (c.touched || c.dirty));
  }

  onSubmit(){
    this.errorMessage = null;
    this.result = null;

    if(this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }
    this.isLoading = true;

    this._shippingCostService.calculateShippingCost(this.form.value as any)
      .subscribe({
        next: (result) =>{
          this.result = result;
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = err?.error?.message ?? "Error while calculating shipping cost";
          this.isLoading = false;
        },
      });
  }
}


