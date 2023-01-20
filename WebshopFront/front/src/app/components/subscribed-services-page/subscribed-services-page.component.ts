import { Component, OnInit } from '@angular/core';
import { PaymentMicroserviceType } from 'src/app/enums/payment-microservice-type';
import { Subscription } from 'src/app/model/subscription';
import { SubscribeService } from 'src/app/services/subscribe.service';

@Component({
  selector: 'app-subscribed-services-page',
  templateUrl: './subscribed-services-page.component.html',
  styleUrls: ['./subscribed-services-page.component.css']
})
export class SubscribedServicesPageComponent implements OnInit {
  subscriptions: Array<Subscription> = []
  types: typeof PaymentMicroserviceType = PaymentMicroserviceType;

  constructor(private subscribeService: SubscribeService) { }

  ngOnInit(): void {
    this.subscribeService.getSubscribedByPort().subscribe(ret => {
      this.subscriptions = ret;
    })
  }

  removeServiceFromSubscriptions(type: PaymentMicroserviceType, index: number) {
    if(type.toString() === PaymentMicroserviceType.Paypal.toString()) {
      this.subscribeService.removeServiceType(0).subscribe(ret => {
        if(ret) {
          alert('Removed successfully!')
          this.subscriptions.splice(index, 1);
        } else
          alert('Removing service type failed!')
      })
    }
    else if(type.toString() === PaymentMicroserviceType.QR.toString()) {
      this.subscribeService.removeServiceType(1).subscribe(ret => {
        if(ret) {
          alert('Removed successfully!')
          this.subscriptions.splice(index, 1);
        } else
          alert('Removing service type failed!')
      })
    }
    else if(type.toString() === PaymentMicroserviceType.Card.toString()) {
      this.subscribeService.removeServiceType(2).subscribe(ret => {
        if(ret) {
          alert('Removed successfully!')
          this.subscriptions.splice(index, 1);
        } else
          alert('Removing service type failed!')
      })
    }
    else {
      this.subscribeService.removeServiceType(3).subscribe(ret => {
        if(ret) {
          alert('Removed successfully!')
          this.subscriptions.splice(index, 1);
        } else
          alert('Removing service type failed!')
      })
    }
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
}
