import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Asset } from "src/app/shared/models/asset.model";
import { DateParam } from "src/app/shared/models/date-param.model";
import { Invoice } from "src/app/shared/models/invoice.model";
import { AssetManagementApiClient } from "src/config/http/Api/asset-management-api-client";

@Injectable({ providedIn: 'root' })
export class InvoiceRepositoryService {
  constructor (
    private http: AssetManagementApiClient
  ) {}

  getInvoice(dateParam: DateParam): Observable<Invoice[]> {
    // return this.http.get('invoice?month=' + dateParam.month + '&year=' + dateParam.year);
    return this.http.post('invoice', dateParam);
  }

  getInvoiceDetails(dateParam: DateParam): Observable<Asset[]> {
    return this.http.post('invoice/get-by-issued-date', dateParam);
  }
}