import { Component, OnInit } from '@angular/core';
import { PaymentService } from 'src/app/services/payment.service';

@Component({
  selector: 'app-logged-company-page',
  templateUrl: './logged-company-page.component.html',
  styleUrls: ['./logged-company-page.component.css']
})
export class LoggedCompanyPageComponent implements OnInit {

  constructor(private paymentService: PaymentService) { }

  ngOnInit(): void {
  }

  makePayment() {
    let monthaccess = document.getElementById('monthaccess') as HTMLInputElement
    let monthsubscribe = document.getElementById('monthsubscribe') as HTMLInputElement
    if(monthaccess.checked) {
      this.paymentService.makePayment(0).subscribe(ret => {

      })
    } else if(monthsubscribe.checked) {
      this.paymentService.makePayment(1).subscribe(ret => {
        
      })
    } else {
      this.paymentService.makePayment(2).subscribe(ret => {
        
      })
    }
  }
}
