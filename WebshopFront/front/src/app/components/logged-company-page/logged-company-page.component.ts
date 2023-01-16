import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SubscriptionOption } from 'src/app/model/subscription-option';
import { PaymentService } from 'src/app/services/payment.service';
import { SubscribeOptionsService } from 'src/app/services/subscribe-options.service';

@Component({
  selector: 'app-logged-company-page',
  templateUrl: './logged-company-page.component.html',
  styleUrls: ['./logged-company-page.component.css']
})
export class LoggedCompanyPageComponent implements OnInit {

  constructor(private router: Router, private subscribeOptionsService: SubscribeOptionsService) { }

  options : Array<SubscriptionOption> = new Array<SubscriptionOption>();
  checked : number = 0;

  ngOnInit(): void {
    this.subscribeOptionsService.getAllSubscriptionOptions().subscribe(response => {
      this.options = response;
    });
  }

  makePayment() {
    this.router.navigate(['/paymentMethod'])
  }

  onOptionSelect(type: number) : void {
    this.checked = type;
  }
}
