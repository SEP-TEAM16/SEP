import { Component, OnInit } from '@angular/core';
import { PaymentMicroserviceType } from 'src/app/enums/payment-microservice-type';
import { Subscription } from 'src/app/model/subscription';
import { SubscriptionOption } from 'src/app/model/subscription-option';
import { PaymentService } from 'src/app/services/payment.service';
import { SubscribeOptionsService } from 'src/app/services/subscribe-options.service';
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
  checkedSub : SubscriptionOption = {} as SubscriptionOption;

  constructor(private paymentService: PaymentService, private subscribeService: SubscribeService , private subscribeOptionsService: SubscribeOptionsService) { }

  ngOnInit(): void {
    this.subscribeService.getSubscribedByPort().subscribe(ret => {
      this.subscriptions = ret;
    })
    this.subscribeOptionsService.getAllSubscriptionOptions().subscribe(response => {
      for(let opt of response){
        if(opt.name === localStorage.getItem('checkedSubs'))
          this.checkedSub = opt
      }
    });
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
    let chosedInput = ""
    for(let val of this.subscriptions) {
      let input = document.getElementById(this.returnEnumValue(val.paymentMicroserviceType)) as HTMLInputElement;
      if(input.checked) {
        chosedInput = this.returnEnumValue(val.paymentMicroserviceType)
        break
      }
    }
    

    if(chosedInput === 'Paypal') {
      this.paymentService.makePayPalPaymentForSubs(this.checkedSub).subscribe(ret => {
        document.location.href = ret;
      })
    } else if(chosedInput === 'Card'){
      this.paymentService.makeCardPaymentForSubs(this.checkedSub).subscribe(ret => {
        document.location.href = ret;
      })
    } else if(chosedInput === 'Bitcoin'){
      this.paymentService.makeBitCoinPaymentForSubs(this.checkedSub).subscribe(ret => {
        document.location.href = ret;
      })
    } else {
      this.paymentService.makeQrCodePaymentForSubs(this.checkedSub).subscribe(ret => {
        document.location.href = ret;
      })
    }
  }

  
}
