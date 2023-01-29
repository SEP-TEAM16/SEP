import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  makePayPalPaymentUrl: string = 'https://localhost:7035/api/payment'
  makeBitCoinPaymentUrl: string = 'https://localhost:7035/api/payment/bitcoin'
  makeQrCodePaymentUrl: string = 'https://localhost:7035/api/payment/qr'
  makeCardPaymentUrl: string = 'https://localhost:7035/api/payment/bank'
  getUnsubscribedUrl: string = 'https://localhost:7035/api/payment/unsubscribed'

  constructor(private http: HttpClient) { }

  public makePayPalPayment(): Observable<any> {
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makePayPalPaymentUrl, null, requestOptions );
  }

  public makeCardPayment(): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makeCardPaymentUrl, null, requestOptions);
  }

  public makeBitCoinPayment(): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }

    return this.http.post<any>(this.makeBitCoinPaymentUrl, null, requestOptions);
  }

  public makeQrCodePayment(): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }

    return this.http.post<any>(this.makeQrCodePaymentUrl, null, requestOptions);
  }
}
