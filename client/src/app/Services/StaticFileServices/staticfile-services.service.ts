import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import * as moment from "moment";

@Injectable({
  providedIn: 'root'
})
export class StaticFileServicesService {

  constructor(private http: HttpClient) {
  }

  loginImageUrl = 'http://localhost:5000/images/LoginPicture.png';
  getLoginImage(): Observable<Blob> {
    return this.http.get(this.loginImageUrl, {responseType: 'blob'});
  }

}
