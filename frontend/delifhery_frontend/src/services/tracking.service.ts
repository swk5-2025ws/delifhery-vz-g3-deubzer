import {Injectable} from '@angular/core';
import {environment} from '../environments/environment';
import {HttpClient} from '@angular/common/http';
import {catchError, map, Observable, of} from 'rxjs';

export interface TrackingStatusRequest {
  trackingNumber: string;
  postalCode: string;
}
export interface TrackingStatusEvent{
  occurredAt: Date;
  status: string;
  location?: string;
  note?:string;
}

export interface TrackingStatusResponse {
  trackingNumber:string;
  sender: string;
  recipient: string;
  history: TrackingStatusEvent[];
}

@Injectable({providedIn: 'root'})
export class TrackingService {
  private url = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  private errorHandler(error: Error | any): Observable<any> {
    console.error(error);
    return of(null);
  }


  trackShipment(request: TrackingStatusRequest): Observable<TrackingStatusResponse | null> {
    const pc= encodeURIComponent(request.postalCode.trim())
    const tn = encodeURIComponent(request.trackingNumber.trim())

    return this.http.get<TrackingStatusResponse>(`${this.url}/api/tracking/${pc}/${tn}`)
      .pipe(map (r =>({
        ...r,
        history: r.history.map(e => ({...e, occurredAt: new Date(e.occurredAt as any)}))
      }))).pipe( catchError(err => this.errorHandler(err)));
  }
}
