import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SubscribeService {
  private makeSubscriptionUrl: string = 'https://localhost:7035/api/subscriptions'

  constructor(private http: HttpClient) { }

  public makeSubscribe(serviceType: number): Observable<Boolean> {
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'senderPort': '7035'
    });
    
    return this.http.post<Boolean>(this.makeSubscriptionUrl, serviceType, {headers: headers});
  }
}
