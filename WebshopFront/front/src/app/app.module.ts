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
import { SubscribedServicesPageComponent } from './components/subscribed-services-page/subscribed-services-page.component';
import { SuccessPageComponent } from './components/success-page/success-page.component';
import { CancelPageComponent } from './components/cancel-page/cancel-page.component';

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
    SubscribedServicesPageComponent,
    SuccessPageComponent,
    CancelPageComponent,
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
