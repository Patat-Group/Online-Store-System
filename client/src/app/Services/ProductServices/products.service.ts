import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Products } from 'src/app/Models/Products';
import { map } from 'rxjs/operators';
import { ITag } from 'src/app/Models/Tags';
import { Pagination } from 'src/app/Models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(private http: HttpClient) { }
  baseUrl = 'http://localhost:5000/api/Product';
  baseUrl2 = 'http://localhost:5000/api/subCategory/category/';

  getProductsWithCategory(categoryIdFilter?: number | any,
    subCategoryIdFilter?: number | any, sortId?: number | any,
    searchProduct?: string | any) {
    let params = new HttpParams();
    if (categoryIdFilter != 0 && categoryIdFilter != null && categoryIdFilter != undefined)
      params = params.append("CategoryIdFilter", categoryIdFilter);
    if (subCategoryIdFilter !== 0)
      params = params.append("SubCategoryIdFilter", subCategoryIdFilter);
    if (sortId == 1)
      params = params.append("SortByNewest", "true");
    if (sortId == 2)
      params = params.append("SortByLowerPrice", "true");
    if (sortId == 3)
      params = params.append("SortByHigerPrice", "true");
    if (searchProduct != null) {
      params = params.append("Search", searchProduct);
    }
    return this.http.get<Pagination>(this.baseUrl, { observe: 'response', params })
      .pipe(
        map(response => {
          return response.body;
        })
      );
  }

  getTags(id: number) {
    return this.http.get<ITag[]>(this.baseUrl2 + id);
  }
}
