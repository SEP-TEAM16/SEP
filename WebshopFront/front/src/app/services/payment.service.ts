import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  makePaymentUrl: string = 'https://localhost:7035/api/payment'

  constructor(private http: HttpClient) { }

  public makePayment(type: number): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
   });

    return this.http.post<any>(this.makePaymentUrl, type, {headers: headers});
  }
}
