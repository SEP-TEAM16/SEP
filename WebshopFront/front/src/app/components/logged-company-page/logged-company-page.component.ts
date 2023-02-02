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
    for(let opt of this.options) {
      let input = document.getElementById(opt.name) as HTMLInputElement;
      if(input.checked) {
        localStorage.setItem('checkedSubs', opt.name)
        alert('Value ' + input.value)
        localStorage.setItem('subsType', input.value)
      }
    }

    this.router.navigate(['/paymentMethod'])
  }

  onOptionSelect(type: number) : void {
    this.checked = type;
  }
}
