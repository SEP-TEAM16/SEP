import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Package } from '../model/package';
import { SubscriptionOption } from '../model/subscription-option';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  makePayPalPaymentForPackageUrl: string = 'https://localhost:7035/api/payment'
  makeBitCoinPaymentForPackageUrl: string = 'https://localhost:7035/api/payment/bitcoin'
  makeQrCodePaymentForPackageUrl: string = 'https://localhost:7035/api/payment/qr'
  makeCardPaymentForPackageUrl: string = 'https://localhost:7035/api/payment/bank'
  makePayPalPaymentForSubsUrl: string = 'https://localhost:7035/api/payment/paypalSubs'
  makeBitCoinPaymentForSubsUrl: string = 'https://localhost:7035/api/payment/bitcoinSubs'
  makeQrCodePaymentForSubsUrl: string = 'https://localhost:7035/api/payment/qrSubs'
  makeCardPaymentForSubsUrl: string = 'https://localhost:7035/api/payment/bankSubs'
  getUnsubscribedUrl: string = 'https://localhost:7035/api/payment/unsubscribed'
  makePayPalSubscriptionUrl: string = 'https://localhost:7035/api/payment/paypal/subscribe'

  constructor(private http: HttpClient) { }

  public makePayPalPaymentForPackage(chosenPackage: Package): Observable<any> {
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makePayPalPaymentForPackageUrl, chosenPackage, requestOptions );
  }

  public makeCardPaymentForPackage(chosenPackage: Package): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });

    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makeCardPaymentForPackageUrl, chosenPackage, requestOptions);
  }

  public makeBitCoinPaymentForPackage(chosenPackage: Package): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }

    return this.http.post<any>(this.makeBitCoinPaymentForPackageUrl, chosenPackage, requestOptions);
  }

  public makeQrCodePaymentForPackage(chosenPackage: Package): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }

    return this.http.post<any>(this.makeQrCodePaymentForPackageUrl, chosenPackage, requestOptions);
  }


  public makePayPalPaymentForSubs(sub: SubscriptionOption): Observable<any> {
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makePayPalPaymentForSubsUrl, sub, requestOptions );
  }

  public makeCardPaymentForSubs(sub: SubscriptionOption): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });

    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makeCardPaymentForSubsUrl, sub, requestOptions);
  }

  public makeBitCoinPaymentForSubs(sub: SubscriptionOption): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }

    return this.http.post<any>(this.makeBitCoinPaymentForSubsUrl, sub, requestOptions);
  }

  public makeQrCodePaymentForSubs(sub: SubscriptionOption): Observable<any> {
    var headers = new HttpHeaders({ 
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }

    return this.http.post<any>(this.makeQrCodePaymentForSubsUrl, sub, requestOptions);
  }

  public makePayPalSubscription(chosenPackage: Package): Observable<any> {
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
      'senderPort': '7035'
    });
    const requestOptions: Object = {
      headers: headers,
      responseType: 'text'
    }
    return this.http.post<any>(this.makePayPalSubscriptionUrl, chosenPackage, requestOptions );
  }
}
