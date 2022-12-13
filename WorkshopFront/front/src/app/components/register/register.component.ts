import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm:any;
  passwordTheSame: boolean = true;

  constructor() { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      "name": new FormControl(null, [Validators.required,Validators.pattern('[A-ZŠĐČĆŽ]{1}[a-zšđčćž]+')]),
      "pib": new FormControl(null, [Validators.required,Validators.pattern('[0-9]{13}')]),
      "email": new FormControl(null, [Validators.required,Validators.email]),
      "phoneNumber": new FormControl(null, [Validators.required,Validators.pattern('[0-9]{6,10}')]),
      "address": new FormControl(null, [Validators.required,Validators.pattern('([A-ZŠĐČĆŽ]{1}[a-zšđčćž]+ )+[0-9]+')]),
      "password": new FormControl(null, [Validators.required,Validators.pattern('[a-zA-Z0-9]*')]),
      "secondPassword": new FormControl(null, [Validators.required,Validators.pattern('[a-zA-Z0-9]*')])
    });
  }

  get name() {
    return this.registerForm.get('name');
  }
  get pib() {
    return this.registerForm.get('pib');
  }
  get email(){
    return this.registerForm.get('email');
  }
  get phoneNumber(){
    return this.registerForm.get('phoneNumber');
  }
  get date() {
    return this.registerForm.get('date');
  }
  get address() {
    return this.registerForm.get('address');
  }
  get password() {
    return this.registerForm.get('password');
  }
  get secondPassword() {
    return this.registerForm.get('secondPassword');
  }

  registerCompany() {}
}
