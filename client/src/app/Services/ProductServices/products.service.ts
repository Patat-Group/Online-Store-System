import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Products } from 'src/app/Models/Products';
import { map } from 'rxjs/operators';
import { ITag } from 'src/app/Models/Tags';
import { getPaginationHeaders, getPaginationResult } from '../PaginationHelper';
import { ProductsParam } from 'src/app/Models/ProductParams';
import { PaginatedResult } from 'src/app/Models/Pagination';
import { ProductsForAdd } from 'src/app/Models/ProductForAdd';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  productParams: ProductsParam;
  baseUrl = 'http://localhost:5000/api/Product';
  baseUrl2 = 'http://localhost:5000/api/subCategory/category/';
  getTagNameUrl = 'http://localhost:5000/api/subCategory/';
  getProductTagsUrl = 'http://localhost:5000/api/LinkProductWithSubCategory/';

  constructor(private http: HttpClient) {
    this.productParams = new ProductsParam();
  }


  getProductsWithCategory(categoryIdFilter?: number | any,
    subCategoryIdFilter?: number | any, sortId?: number | any,
    searchProduct?: string | any, currentPage?: number| any,
    itemsPerPage?: number | any,UserFilter?: string | any,): Observable<PaginatedResult<Products[]>> {

    const paginatedResult: PaginatedResult<Products[]> = new PaginatedResult<Products[]>();

    let params = new HttpParams();
    if (currentPage != null && itemsPerPage != null) {
      params = params.append('pageNumber', currentPage);
      params = params.append('pageSize', itemsPerPage);
      // console.log("waiting......");
    }
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
    if (UserFilter != null) {
      params = params.append("UserFilter", UserFilter);
    }

    return this.http.get<Products[]>(this.baseUrl, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          // console.log("headers2  " + (response.headers.get('Pagination')));
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination')!);
          }
          return paginatedResult;
        })
      );
  }

  AddProduct(productForAdd: ProductsForAdd): Observable<number> {
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    // console.log("data for add product: " + productForAdd);
    return this.http.post<number>(this.baseUrl + '/addProduct', productForAdd, httpOptions).pipe();
  }

  setProductsParams(params: ProductsParam) {
    this.productParams = params;
  }
  deleteProduct(id: number): Observable<any> {
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    return this.http.delete(this.baseUrl +"/"+ id, httpOptions).pipe();
  }
  getTags(id: number) {
    return this.http.get<ITag[]>(this.baseUrl2 + id);
  }
  getProductTags(id: number) {
    return this.http.get(this.getProductTagsUrl + id);
  }
  getTagName(id: number): Observable<any> {
    console.log(this.getTagNameUrl + id);
    return this.http.get(this.getTagNameUrl + id);
  }
}
