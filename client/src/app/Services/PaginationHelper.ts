import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../Models/Pagination';

export function getPaginationResult<T>(url: any, params: any, http: HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
            paginatedResult.result = response.body;
            if (response.headers.get('Pagination') !== null) {
                paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') || '{}');
            }
            return paginatedResult;
        })
    );

}

``
export function getPaginationHeaders(currentPage: number, totalItems: number) {
    let params = new HttpParams();

    params = params.append('currentPage', currentPage.toString());
    params = params.append('totalItems', totalItems.toString());

    return params;
}