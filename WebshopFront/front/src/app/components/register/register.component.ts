import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Company } from 'src/app/model/company';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm:any;
  passwordTheSame: boolean = true;
  userExists: boolean = false;
  dateExists: boolean = true;
  company: Company = {} as Company;

  constructor() { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      "name": new FormControl(null, [Validators.required,Validators.pattern('([A-ZŠĐČĆŽ]{1}[a-zšđčćž]+ *)+')]),
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
  get address() {
    return this.registerForm.get('address');
  }
  get password() {
    return this.registerForm.get('password');
  }
  get secondPassword() {
    return this.registerForm.get('secondPassword');
  }

  registerCompany() {
    let date = document.getElementById('establishment') as HTMLInputElement;
    if(date.value === '') {
      this.dateExists = false
    } else
      this.dateExists = true

    if(this.registerForm.get('password').value === this.registerForm.get('secondPassword').value) {
      this.passwordTheSame = true;
      
    }else{
      this.passwordTheSame = false;
      return;
    }
  }
}
