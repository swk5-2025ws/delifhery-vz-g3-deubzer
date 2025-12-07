import {Injectable} from '@angular/core';
import {environment} from '../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

export interface ShippingPriceResponse {
    totalPrice: number;
    currency: string;
    basePrice: number;
    bundeslandSurcharge: number;
    seasonalDiscount: number;
}

export interface ShippingPriceRequest{
  senderPostalCode: string;
  senderCity: string;
  senderStreet: string;
  senderHouseNumber: string;

  recipientPostalCode: string;
  recipientCity: string;
  recipientStreet: string;
  recipientHouseNumber: string;

  widthCm: number;
  heightCm: number;
  lengthCm: number;
  weightKg: number;
}

@Injectable({providedIn: 'root'})
export class ShippingCostService {
  private readonly Url = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  calculateShippingCost(request: ShippingPriceRequest): Observable<ShippingPriceResponse> {
    return this.http.post<ShippingPriceResponse>(`${this.Url}/api/shipping/calculate`,request);
  }
}
