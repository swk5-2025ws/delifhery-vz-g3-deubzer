import {Injectable} from '@angular/core';
import {environment} from '../environments/environment';
import {HttpClient} from '@angular/common/http';
import {catchError, map, Observable, of} from 'rxjs';

export interface ContactMethod{
  contactId : number,
  customerId: number,
  type: string,
  value: string,
  isVerified: boolean,
}

@Injectable({providedIn: 'root'})
export class ContactMethodService {
  private Url = `${environment.apiBaseUrl}/api/customers/me/contactMethod`;

  constructor(private http: HttpClient) {}

  private errorHandler(error: Error | any): Observable<any> {
    console.error(error);
    return of(null);
  }

  getForCurrentUser(): Observable<ContactMethod[]> {
    return this.http.get<ContactMethod[]>(this.Url)
      .pipe(map<any, ContactMethod[]>(res =>res['contactMethods']),catchError(this.errorHandler));
  }
}
