import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Package } from '../model/package';

@Injectable({
  providedIn: 'root'
})
export class PackageService {
  private getAllUrl: string = 'https://localhost:7035/api/package'

  constructor(private http: HttpClient) { }

  public getAll(): Observable<any> {
    var headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.get<Array<Package>>(this.getAllUrl, { headers: headers });
  }
}
