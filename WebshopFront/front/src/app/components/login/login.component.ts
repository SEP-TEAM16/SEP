import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WebShopUser } from 'src/app/model/webshopuser';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  notFounded: Boolean = false;
  user: WebShopUser = {} as WebShopUser;

  constructor(private userService: UserService, private router: Router) { }

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
      console.log('token from back: ' + ret.token)
      console.log('token from storage: ' + localStorage.getItem('token'))
      if(ret.userType === 1)
        this.router.navigate(['loggedCompany'])
      else
        this.router.navigate(['servicesPage'])
    }, _ => {
      this.notFounded = true
    })
  }

}
