import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IProductImages } from 'src/app/Models/ProductImages';

@Injectable({
  providedIn: 'root'
})
export class ProductImagesService {
  baseUrl = 'http://localhost:5000/api/Product/';
  constructor(private http: HttpClient) { }

  getProduct(id: number): Observable<IProductImages> {
    return this.http.get<IProductImages>(this.baseUrl + id).pipe();
  }
}
