import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPageComponent } from './components/admin-page/admin-page.component';
import { CancelPageComponent } from './components/cancel-page/cancel-page.component';
import { ChoosePaymentTypePageComponent } from './components/choose-payment-type-page/choose-payment-type-page.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LoggedCompanyPageComponent } from './components/logged-company-page/logged-company-page.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ServicesPageComponent } from './components/services-page/services-page.component';
import { SubscribedServicesPageComponent } from './components/subscribed-services-page/subscribed-services-page.component';
import { SuccessPageComponent } from './components/success-page/success-page.component';

const routes: Routes = [
  { path: '', component: HomePageComponent},
  { path: 'login', component: LoginComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'loggedCompany', component: LoggedCompanyPageComponent},
  { path: 'servicesPage', component: ServicesPageComponent},
  { path: 'paymentMethod', component: ChoosePaymentTypePageComponent},
  { path: 'adminPage', component: AdminPageComponent},
  { path: 'subscribedServices', component: SubscribedServicesPageComponent},
  { path: 'success', component: SuccessPageComponent},
  { path: 'cancel', component: CancelPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
