import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SubscriptionOption } from '../model/subscription-option';

@Injectable({
  providedIn: 'root'
})
export class SubscribeOptionsService {
  getAllSubscriptionOptionsUrl: string = 'https://localhost:7035/api/subscription-options/'

  constructor(private http: HttpClient) { }

  public getAllSubscriptionOptions(): Observable<any> {
    var headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.get<Array<SubscriptionOption>>(this.getAllSubscriptionOptionsUrl, { headers: headers });
  }
}
