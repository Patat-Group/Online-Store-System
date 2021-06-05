import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VipAds } from 'src/app/Models/VipAds';

@Injectable({
  providedIn: 'root'
})
export class VipService {

  constructor(private http: HttpClient) { }
  baseUrl = 'http://localhost:5000/api/vipads/';

  GetVips(): Observable<VipAds[]> {
    return this.http.get<VipAds[]>(this.baseUrl).pipe();
  }
}
