import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ChangeDetectorRef, Component, OnInit, TemplateRef } from '@angular/core';
import { InvoiceService } from '../core/services/invoice.service';
import { DateParam } from '../shared/models/date-param.model';
import { Invoice } from '../shared/models/invoice.model';
import { combineLatest, Subscription } from 'rxjs';
import { Asset } from '../shared/models/asset.model';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html',
  styleUrls: ['./invoices.component.scss']
})
export class InvoicesComponent implements OnInit { 
  invoices: Invoice[] = [];
  modalRef?: BsModalRef;

  assets: Asset[] = [];

  activeInvoice?: Invoice;
  subscriptions: Subscription[] = [];
  searchForm!: FormGroup;

  isvalidYear = true;
  isvalidMonth = true;

  constructor(
    private invoiceService: InvoiceService,
    private modalService: BsModalService,
    private changeDetection: ChangeDetectorRef,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    //this.loadInvoice();
    this.buildSearchForm();
  }

  loadInvoice(dateParam: DateParam): void {
    this.invoiceService.getInvoice(dateParam).subscribe({
      next: (data) => {
        this.invoices = data;
      }
    });
  }

  loadInvoiceByDate(template: TemplateRef<any>, invoice: Invoice): void {
    let dateParam = new DateParam();
    dateParam.date = invoice.issuedDateAsString;

    this.activeInvoice = invoice;

    this.invoiceService.getInvoiceDetails(dateParam).subscribe({
      next: (data) => {
        let assets: Asset[] = [];

        data.forEach((asset: Asset) => {
          asset.validFrom = asset.validFrom == null ? 'NULL' : new Date(asset.validFrom).toISOString().substring(0, 10);
          asset.validTo   = asset.validTo   == null ? 'NULL' : new Date(asset.validTo).toISOString().substring(0, 10);

          assets.push(asset);
        });

        this.assets = assets;
      }
    });

    const _combine = combineLatest(
      this.modalService.onShow,
      this.modalService.onShown,
      this.modalService.onHide,
      this.modalService.onHidden
    ).subscribe(() => this.changeDetection.markForCheck());
 
    this.subscriptions.push(
      this.modalService.onHide.subscribe(() => {
        this.assets = [];
      })
    );
 
    this.subscriptions.push(_combine);
    this.modalRef = this.modalService.show(template);
  }

  private buildSearchForm(): void {
    const abstractControlOptions = {
      updateOn: 'submit'
    }

    this.searchForm = this.formBuilder.group(
      { 
        month: ['', [Validators.required]],
        year: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(4)]]
      },
      abstractControlOptions as AbstractControlOptions
    );
  }

  submitForm(): void {
    this.isvalidMonth = true;
    this.isvalidYear = true;

    if (this.validateAssetForm()) {
      this.loadInvoice(this.searchForm.value);
    } else {
      Swal.fire('Error', 'Invalid year or month. Please check your entries.', 'error');
    }
  }

  private validateAssetForm() : boolean {
    if (this.searchForm.valid)
      return true;

    if (this.searchForm.get('month')?.hasError('required'))
      this.isvalidMonth = false;

    if (this.searchForm.get('year')?.hasError('required') || 
        this.searchForm.get('year')?.hasError('minLength') || 
        this.searchForm.get('year')?.hasError('maxLength'))
      this.isvalidYear = false;

    return false;
  }
}
