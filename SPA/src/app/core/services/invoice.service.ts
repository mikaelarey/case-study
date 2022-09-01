import { Injectable } from "@angular/core";
import { catchError, map, Observable, throwError} from "rxjs";

import Swal from 'sweetalert2';
import { InvoiceRepositoryService } from "../repository/invoice.repository";
import { DateParam } from "src/app/shared/models/date-param.model";

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  constructor(
    private invoiceRepositoryService: InvoiceRepositoryService
  ) {}

  getInvoice(dateParam: DateParam): Observable<any> {
    return this.invoiceRepositoryService.getInvoice(dateParam).pipe(
      catchError((error) => {
        this.setErrorMessages(error);
        return throwError(() => new Error(error));
      }),
      map((data) => {
        console.log(data);
        return data;
      })
    );
  }

  getInvoiceDetails(dateParam: DateParam): Observable<any> {
    return this.invoiceRepositoryService.getInvoiceDetails(dateParam).pipe(
      catchError((error) => {
        this.setErrorMessages(error);
        return throwError(() => new Error(error));
      }),
      map((data) => {
        console.log(data);
        return data;
      })
    );
  }

  private setErrorMessages(error: any) {
    Swal.fire(error.statusText, "Unable to get data from the server");
  }
}