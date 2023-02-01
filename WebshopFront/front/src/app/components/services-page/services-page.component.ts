import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PaymentMicroserviceType } from 'src/app/enums/payment-microservice-type';
import { Package } from 'src/app/model/package';
import { Subscription } from 'src/app/model/subscription';
import { PackageService } from 'src/app/services/package.service';
import { PaymentService } from 'src/app/services/payment.service';
import { SubscribeService } from 'src/app/services/subscribe.service';

@Component({
  selector: 'app-services-page',
  templateUrl: './services-page.component.html',
  styleUrls: ['./services-page.component.css']
})
export class ServicesPageComponent implements OnInit {
  packages: Array<Package> = []
  selectedPackage: Package = {} as Package;
  subscriptions: Array<Subscription> = []
  privateKey: String = '';

  constructor(private packageService: PackageService, private paymentService: PaymentService, 
    private subscribeService: SubscribeService, private router: Router) { }

  ngOnInit(): void {
    this.packageService.getAll().subscribe(ret => {
      this.packages = ret;
    })
    this.subscribeService.getSubscribedByPort().subscribe(ret => {
      this.subscriptions = ret;
    })
    let input = document.getElementById('private') as HTMLInputElement
    input.hidden = true
  }

  buyPackage() {
    if(this.selectedPackage.name === undefined) {
      alert('You need to select package!')
      return;
    }

    if(this.selectedPackage.name === 'Subscribe to paypal') 
      this.makeSubscriptionToPaypal()
    else {
      let select = document.getElementById('method') as HTMLSelectElement;

      if(select.value === 'Paypal') {
        this.paymentService.makePayPalPaymentForPackage(this.selectedPackage).subscribe(ret => {
          document.location.href = ret;
        })
      } else if(select.value === 'Card'){
        this.paymentService.makeCardPaymentForPackage(this.selectedPackage).subscribe(ret => {
          document.location.href = ret;
        })
      } else if(select.value === 'Bitcoin'){
        let input = document.getElementById('private') as HTMLInputElement
        if(input.value === '' || input.value === undefined) {
          alert('Enter private key')
          return;
        }
        this.paymentService.makeBitCoinPaymentForPackage(this.selectedPackage, input.value).subscribe(ret => {
          this.router.navigate(['success'])
        })
      } else {
        this.paymentService.makeQrCodePaymentForPackage(this.selectedPackage).subscribe(ret => {
          document.location.href = ret;
        })
      }
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

  methodSelected(selected: Package) {
    this.selectedPackage = selected;
    let select = document.getElementById('method') as HTMLSelectElement
    if(selected.name === 'Subscribe to paypal')
      select.hidden = true
    else
      select.hidden = false
  }

  makeSubscriptionToPaypal() {
    this.paymentService.makePayPalSubscription(this.selectedPackage).subscribe(ret => {
      document.location.href = ret;
    })
  }

  methodChanged() {
    let select = document.getElementById('method') as HTMLSelectElement;
    let input = document.getElementById('private') as HTMLInputElement
    if(select.value === 'Bitcoin')
      input.hidden = false
    else
      input.hidden = true
  }
}
