import {Injectable} from '@angular/core';
import {environment} from '../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

export interface ContactMethod{
  contactId : number,
  customerId: number,
  type: string,
  value: string,
  isVerified: boolean,
}

@Injectable({providedIn: 'root'})
export class ContactMethodService {
  private Url = `${environment.apiBaseUrl}/api/customers/currentUser/contactMethod`;

  constructor(private http: HttpClient) {}

  getForCurrentUser(): Observable<ContactMethod[]> {
    return this.http.get<ContactMethod[]>(this.Url);
  }
}
