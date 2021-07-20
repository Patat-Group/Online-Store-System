import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Products } from 'src/app/Models/Products';
import { PaginatedResult } from 'src/app/Models/Pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(private http: HttpClient) { }
  baseUrl = 'http://localhost:5000/api/Product/';

  GetProducts(page?: any, itemPerPage?: any): Observable<PaginatedResult<Products[]>> {
    const paginationResult: PaginatedResult<Products[]> = new PaginatedResult<Products[]>();
    let params = new HttpParams();
    if (page != null && itemPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemPerPage);
    }
    //   }
    //   if (CategoryIdFilter) {
    //     params = params.append('CategoryIdFilter', CategoryIdFilter.toString());
    //   }
    return this.http.get<PaginatedResult<Products[]>>(this.baseUrl, { observe: 'response', params })
      .pipe(
        map(response => {
          paginationResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginationResult.pagination = JSON.parse(response.headers.get('Pagination'))
          }
          return paginationResult;
        })
      );
  }


  getProductsWithCategory(id: number): Observable<Products[]> {
    return this.http.get<Products[]>(this.baseUrl + 'category/' + id);
  }
}
