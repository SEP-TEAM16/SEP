import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChoosePaymentTypePageComponent } from './components/choose-payment-type-page/choose-payment-type-page.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LoggedCompanyPageComponent } from './components/logged-company-page/logged-company-page.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ServicesPageComponent } from './components/services-page/services-page.component';

const routes: Routes = [
  { path: '', component: HomePageComponent},
  { path: 'login', component: LoginComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'loggedCompany', component: LoggedCompanyPageComponent},
  { path: 'servicesPage', component: ServicesPageComponent},
  { path: 'paymentMethod', component: ChoosePaymentTypePageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
