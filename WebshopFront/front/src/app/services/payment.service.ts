import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Package } from '../model/package';

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

  public makePayPalPayment(chosenPackage: Package): Observable<any> {
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makePayPalPaymentUrl, chosenPackage, requestOptions );
  }

  public makeCardPayment(chosenPackage: Package): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });

    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makeCardPaymentUrl, chosenPackage, requestOptions);
  }

  public makeBitCoinPayment(chosenPackage: Package): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }

    return this.http.post<any>(this.makeBitCoinPaymentUrl, chosenPackage, requestOptions);
  }

  public makeQrCodePayment(chosenPackage: Package): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }

    return this.http.post<any>(this.makeQrCodePaymentUrl, chosenPackage, requestOptions);
  }
}
