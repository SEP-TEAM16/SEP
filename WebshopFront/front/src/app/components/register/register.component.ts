import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { WebShopUser } from 'src/app/model/company';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm:any;
  passwordTheSame: boolean = true;
  userExists: boolean = false;
  user: WebShopUser = {} as WebShopUser;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      "name": new FormControl(null, [Validators.required,Validators.pattern('([A-ZŠĐČĆŽ]{1}[a-zšđčćž]+ *)+')]),
      "email": new FormControl(null, [Validators.required,Validators.email]),
      "username": new FormControl(null, [Validators.required,Validators.pattern('([A-ZŠĐČĆŽa-zšđčćž0-9]+)')]),
      "city": new FormControl(null, [Validators.required,Validators.pattern('[A-ZŠĐČĆŽ]{1}[a-zšđčćž]+( [A-ZŠĐČĆŽa-zšđčćž]{1}[a-zšđčćž]*)*')]),
      "street": new FormControl(null, [Validators.required,Validators.pattern('([A-ZŠĐČĆŽ]{1}[a-zšđčćž]+ )+[0-9]+')]),
      "password": new FormControl(null, [Validators.required,Validators.pattern('[a-zA-Z0-9]{8,50}')]),
      "secondPassword": new FormControl(null, [Validators.required,Validators.pattern('[a-zA-Z0-9]{8,50}')])
    });
  }

  get name() {
    return this.registerForm.get('name');
  }
  get username() {
    return this.registerForm.get('username');
  }
  get email(){
    return this.registerForm.get('email');
  }
  get city(){
    return this.registerForm.get('city');
  }
  get street() {
    return this.registerForm.get('street');
  }
  get password() {
    return this.registerForm.get('password');
  }
  get secondPassword() {
    return this.registerForm.get('secondPassword');
  }

  registerUser() {
    if(this.registerForm.get('password').value === this.registerForm.get('secondPassword').value) {
      this.passwordTheSame = true;
      let type = document.getElementById('userType') as HTMLSelectElement
      this.user.userType = parseInt(type.value)
      this.userService.registerUser(this.user).subscribe(ret => {
        console.log(ret)
        alert('Successfully registered!')
        this.router.navigate(['/'])
      })
    } else{
      this.passwordTheSame = false;
      return;
    }
  }
}
