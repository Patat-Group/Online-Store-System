import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from 'src/app/Models/Category';

@Injectable({
  providedIn: 'root'
})
export class CategoryServicesService {

  constructor(private http: HttpClient) { }
  baseUrl = 'http://localhost:5000/api/Category/';

  GetCategories(): Observable<Category[]>{
    return this.http.get<Category[]>(this.baseUrl).pipe();
  }
  GetCategoryById(id :number){
    return this.http.get<Category>(this.baseUrl + id).pipe();
  }
}
