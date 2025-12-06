import {Injectable} from '@angular/core';
import {environment} from '../environments/environment';
import {HttpClient} from '@angular/common/http';
import {catchError, map, Observable, of} from 'rxjs';
import {Router} from '@angular/router';

export interface ContactMethod{
  contactId : number,
  customerId: number,
  type: string,
  value: string,
  isPrimary: boolean,
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
      .pipe(catchError(this.errorHandler));
  }

  createContactMethod(contact: Partial<ContactMethod>): Observable<ContactMethod> {
    return this.http.post<ContactMethod>(this.Url, contact)
      .pipe(catchError(this.errorHandler));
  }

  deleteContact(contactId: number): Observable<void> {
    return this.http.delete<void>(`${this.Url}/${contactId}`)
      .pipe(catchError(this.errorHandler));
  }
}
