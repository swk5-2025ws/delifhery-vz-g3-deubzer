import {environment} from '../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Injectable} from '@angular/core';

export interface ShipmentResponse{
  trackingNumber: string;
  price: number;
  currency: string;
  paymentUrl: string;
  labelImage: string;
  basePrice?: number;
  bundeslandSurcharge?: number;
  seasonalDiscount?: number;
}

export interface ShipmentRequest{
  SenderName: string;
  SenderPostalCode: string;
  SenderStreet: string;
  SenderCity: string;
  RecipientName: string;
  RecipientPostalCode: string;
  RecipientStreet: string;
  RecipientCity: string;
  WidthCm: number;
  HeightCm: number;
  LengthCm: number;
  WeightKg: number;
}

@Injectable({providedIn: 'root'})
export class ShipmentService {
  private readonly Url = environment.apiBaseUrl;
  constructor(private http: HttpClient) {}

  createShipment(request: ShipmentRequest): Observable<ShipmentResponse> {
    return this.http.post<ShipmentResponse>(`${this.Url}/api/shipments`, request);
  }
}
