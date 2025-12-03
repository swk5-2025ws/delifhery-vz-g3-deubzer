import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CustomerInitService {
  private baseUrl = `${environment.apiBaseUrl}/api/customers`;

  constructor(private http: HttpClient) {}

  ensureCurrentCustomer() {
    return this.http.post(`${this.baseUrl}`, {});
  }
}
