import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WebShopUser } from 'src/app/model/webshopuser';
import { SubscribeService } from 'src/app/services/subscribe.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  notFounded: Boolean = false;
  user: WebShopUser = {} as WebShopUser;

  constructor(private userService: UserService, private router: Router, private subscriptionService: SubscribeService) { }

  ngOnInit(): void {
    this.user.emailAddress = ''
    this.user.city = ''
    this.user.street = ''
    this.user.name = ''
    this.user.username = ''
    this.user.password = ''
  }

  loginUser() {
    this.userService.loginUser(this.user).subscribe(ret => {
      this.notFounded = false
      localStorage.setItem('token', ret.token) 
      if(ret.userType === 1) {
        this.subscriptionService.isCompanySubscribed(ret).subscribe(ret => {
          if(ret)
            this.router.navigate(['servicesPage'])
          else
            this.router.navigate(['loggedCompany'])
        })
      } else {
        if(ret.username === 'admin')
          this.router.navigate(['adminPage'])
        else
          this.router.navigate(['servicesPage'])
      }
    }, _ => {
      this.notFounded = true
    })
  }

}
