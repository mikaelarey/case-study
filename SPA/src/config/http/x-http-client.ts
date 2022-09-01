import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

export interface IRequestOptions {
    headers?: HttpHeaders;
    observe?: 'body';
    params?: HttpParams;
    reportProgress?: boolean;
    withCredentials?: boolean;
    body?: any;
}
  
export class XHttpClient {
    constructor(
      public http: HttpClient,
      protected baseUrl: string
    ) { }
  
    get<T>(endpoint: string, params?: object, options?: IRequestOptions): Observable<T> {
        return this.http.get<T>(this.baseUrl + endpoint, params);
    }
  
    post<T>(endpoint: string, params: object, options?: IRequestOptions): Observable<T> {
        return this.http.post<T>(this.baseUrl + endpoint, params, options);
    }
    
    put<T>(endpoint: string, params: object, options?: IRequestOptions): Observable<T> {
        return this.http.put<T>(this.baseUrl + endpoint, params, options);
    }
    
    delete<T>(endpoint: string, options?: IRequestOptions): Observable<T> {
        return this.http.delete<T>(this.baseUrl + endpoint, options);
    }
}
