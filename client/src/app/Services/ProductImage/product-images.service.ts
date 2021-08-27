import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ImageForAdd } from 'src/app/Models/ImageForAdd';
import { IProductImages } from 'src/app/Models/ProductImages';

@Injectable({
  providedIn: 'root'
})
export class ProductImagesService {
  baseUrl = 'http://localhost:5000/api/Product/';
  baseUrl2 = 'http://localhost:5000/api/';

  constructor(private http: HttpClient) { }

  getProduct(id: number): Observable<IProductImages> {
    return this.http.get<IProductImages>(this.baseUrl + id).pipe();
  }
  addImage(id: number, imageForAdd: File) {
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    const formData = new FormData();
    formData.append('File', imageForAdd, imageForAdd.name);
    return this.http.post(this.baseUrl2 + id + '/Image', formData, httpOptions);
  }
  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
