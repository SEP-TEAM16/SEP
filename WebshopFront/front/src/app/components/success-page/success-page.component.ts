import { Component, OnInit } from '@angular/core';
import { SubscribeService } from 'src/app/services/subscribe.service';

@Component({
  selector: 'app-success-page',
  templateUrl: './success-page.component.html',
  styleUrls: ['./success-page.component.css']
})
export class SuccessPageComponent implements OnInit {

  constructor(private subscribeService: SubscribeService) { }

  ngOnInit(): void {
    if (localStorage.getItem('subs') === 'yes') {
      this.subscribeService.subscribeCompany().subscribe(ret => {
        
      })
    }
  }

}
