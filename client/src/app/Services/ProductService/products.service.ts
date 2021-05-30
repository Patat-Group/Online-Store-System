import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Products } from 'src/app/Models/Products';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(private http: HttpClient) { }
  baseUrl = 'http://localhost:5000/api/Product/';

  GetProducts(): Observable<Products[]> {
    return this.http.get<Products[]>(this.baseUrl).pipe();
  }
}
