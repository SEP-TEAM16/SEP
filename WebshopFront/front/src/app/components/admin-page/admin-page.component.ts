import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { PaymentService } from 'src/app/services/payment.service';
import { SubscribeService } from 'src/app/services/subscribe.service';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent implements OnInit {
  selectedMethod: number = -1;
  methods: Array<String> = []

  constructor(private subscribeService: SubscribeService, private router: Router) { }

  ngOnInit(): void {
    this.methods.push('Paypal')
    this.methods.push('QR')
    this.methods.push('Card')
    this.methods.push('Bitcoin')
  }

  subscribe() {
    if(this.selectedMethod === -1) {
      alert('Select method')
      return
    }

    this.subscribeService.makeSubscribe(this.selectedMethod).subscribe(ret => {
      if(ret)
        alert('Subscribe successfully ended!')
      else
        alert('You are already subscribed on this payment method!')
    })
  }

  seeMethods() {
    this.router.navigate(['/subscribedServices'])
  }
}
