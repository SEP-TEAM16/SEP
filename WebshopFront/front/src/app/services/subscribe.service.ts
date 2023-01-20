import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Subscription } from '../model/subscription';

@Injectable({
  providedIn: 'root'
})
export class SubscribeService {
  private makeSubscriptionUrl: string = 'https://localhost:7035/api/subscriptions'
  private getSubscribedByPortUrl: string = 'https://localhost:7035/api/subscriptions/subscribedByPort'
  private removeServiceTypeUrl: string = 'https://localhost:7035/api/subscriptions/removeServiceType'

  constructor(private http: HttpClient) { }

  public makeSubscribe(serviceType: number): Observable<Boolean> {
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'senderPort': '7035'
    });
    
    return this.http.post<Boolean>(this.makeSubscriptionUrl, serviceType, {headers: headers});
  }

  public getSubscribedByPort(): Observable<Array<Subscription>> {
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'senderPort': '7035'
    });
    
    return this.http.get<Array<Subscription>>(this.getSubscribedByPortUrl, {headers: headers});
  }

  public removeServiceType(type: number): Observable<Boolean> {

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'senderPort': '7035'
    });
    
    return this.http.post<Boolean>(this.removeServiceTypeUrl, type, {headers: headers});
  }
}
