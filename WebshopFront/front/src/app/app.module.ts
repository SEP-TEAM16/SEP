import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoggedCompanyPageComponent } from './components/logged-company-page/logged-company-page.component';
import { ServicesPageComponent } from './components/services-page/services-page.component';
import { ChoosePaymentTypePageComponent } from './components/choose-payment-type-page/choose-payment-type-page.component';
import { AdminPageComponent } from './components/admin-page/admin-page.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomePageComponent,
    LoggedCompanyPageComponent,
    ServicesPageComponent,
    ChoosePaymentTypePageComponent,
    AdminPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,  
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
