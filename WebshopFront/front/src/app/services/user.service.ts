import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WebShopUser } from 'src/app/model/webshopuser';
import { AuthenticationResponse } from '../model/authentication-response';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  registerUserUrl: string = 'https://localhost:7035/api/users';
  loginUserUrl: string = 'https://localhost:7035/api/users/auth';

  constructor(private http: HttpClient) { 
  }

  public registerUser(user: WebShopUser): Observable<WebShopUser> {
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');

    return this.http.post<WebShopUser>(this.registerUserUrl, user, {headers: headers});
  }

  public loginUser(user: WebShopUser): Observable<AuthenticationResponse> {
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');

    return this.http.post<AuthenticationResponse>(this.loginUserUrl, user, {headers: headers});
  }
}
