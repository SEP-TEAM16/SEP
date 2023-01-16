import { Component, OnInit } from '@angular/core';
import { PaymentService } from 'src/app/services/payment.service';

@Component({
  selector: 'app-choose-payment-type-page',
  templateUrl: './choose-payment-type-page.component.html',
  styleUrls: ['./choose-payment-type-page.component.css']
})
export class ChoosePaymentTypePageComponent implements OnInit {
  methods: Array<String> = []
  method: String = ""

  constructor(private paymentService: PaymentService) { }

  ngOnInit(): void {
    this.methods.push('Card')
    this.methods.push('QrCode')
    this.methods.push('PayPal')
    this.methods.push('BitCoin')
  }

  methodSelected(method: String) {
    this.method = method;
  }

  makePayment() {
    if(this.method === 'PayPal') {
      this.paymentService.makePayPalPayment().subscribe(ret => {

      })
    } else if(this.method === 'Card'){
      this.paymentService.makeCardPayment().subscribe(ret => {

      })
    } else if(this.method === 'BitCoin'){
      this.paymentService.makeBitCoinPayment().subscribe(ret => {

      })
    } else {
      this.paymentService.makeQrCodePayment().subscribe(ret => {

      })
    }
  }
}
