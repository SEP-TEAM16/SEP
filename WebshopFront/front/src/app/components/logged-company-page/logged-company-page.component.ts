import { Component, OnInit } from '@angular/core';
import { SubscriptionOption } from 'src/app/model/subscription-option';
import { PaymentService } from 'src/app/services/payment.service';
import { SubscribeOptionsService } from 'src/app/services/subscribe-options.service';

@Component({
  selector: 'app-logged-company-page',
  templateUrl: './logged-company-page.component.html',
  styleUrls: ['./logged-company-page.component.css']
})
export class LoggedCompanyPageComponent implements OnInit {

  constructor(private paymentService: PaymentService, private subscribeOptionsService: SubscribeOptionsService) { }

  options : Array<SubscriptionOption> = new Array<SubscriptionOption>();
  checked : number = 2;

  ngOnInit(): void {
    this.subscribeOptionsService.getAllSubscriptionOptions().subscribe(response => {
      this.options = response;
    });
  }

  makePayment() {
    this.paymentService.makePayment(this.checked).subscribe(ret => {

    })
    
  }

  onOptionSelect(type: number) : void {
    this.checked = type;
  }
}
