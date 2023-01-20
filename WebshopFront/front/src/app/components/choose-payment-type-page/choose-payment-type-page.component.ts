import { Component, OnInit } from '@angular/core';
import { PaymentMicroserviceType } from 'src/app/enums/payment-microservice-type';
import { Subscription } from 'src/app/model/subscription';
import { PaymentService } from 'src/app/services/payment.service';
import { SubscribeService } from 'src/app/services/subscribe.service';

@Component({
  selector: 'app-choose-payment-type-page',
  templateUrl: './choose-payment-type-page.component.html',
  styleUrls: ['./choose-payment-type-page.component.css']
})
export class ChoosePaymentTypePageComponent implements OnInit {
  methods: Array<String> = []
  selectedSubs: Subscription = {} as Subscription;
  subscriptions: Array<Subscription> = []

  constructor(private paymentService: PaymentService, private subscribeService: SubscribeService) { }

  ngOnInit(): void {
    this.subscribeService.getSubscribedByPort().subscribe(ret => {
      this.subscriptions = ret;
    })
  }

  returnEnumValue(type: PaymentMicroserviceType) {
    if(type.toString() === PaymentMicroserviceType.Paypal.toString())
      return 'Paypal'
    else if(type.toString() === PaymentMicroserviceType.QR.toString())
      return 'QR'
    else if(type.toString() === PaymentMicroserviceType.Card.toString())
      return 'Card'
    else
      return 'Bitcoin'
  }

  methodSelected(subs: Subscription) {
    this.selectedSubs = subs;
  }

  makePayment() {
    // if(this.method === 'PayPal') {
    //   this.paymentService.makePayPalPayment().subscribe(ret => {

    //   })
    // } else if(this.method === 'Card'){
    //   this.paymentService.makeCardPayment().subscribe(ret => {

    //   })
    // } else if(this.method === 'BitCoin'){
    //   this.paymentService.makeBitCoinPayment().subscribe(ret => {

    //   })
    // } else {
    //   this.paymentService.makeQrCodePayment().subscribe(ret => {

    //   })
    // }
  }
}
