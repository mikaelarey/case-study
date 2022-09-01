import { Component, OnInit, TemplateRef, ChangeDetectorRef } from '@angular/core';
import { combineLatest, Subscription, switchAll } from 'rxjs';
import { AssetService } from '../core/services/assets.service';
import { Asset } from '../shared/models/asset.model';

import Swal from 'sweetalert2';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-assets',
  templateUrl: './assets.component.html',
  styleUrls: ['./assets.component.scss']
})
export class AssetsComponent implements OnInit { 
  assetForm!: FormGroup;
  assets: Asset[] = [];

  modalRef?: BsModalRef;
  subscriptions: Subscription[] = [];
  messages: string[] = [];

  isvalidName = true;
  isvalidPrice = true;
  isAddForm = true;
  activeAssetId = 0;

  constructor(
    private formBuilder: FormBuilder,
    private assetService: AssetService,
    private modalService: BsModalService, 
    private changeDetection: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.assetService.getAssets().subscribe({
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

    this.buildAssetForm();
  }

  openDeleteConfirmation(assetId: any) {
    Swal.fire({
      title: 'Are you sure do you want to delete this asset with an id ' + assetId + '?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.assetService.deleteAsset(assetId).subscribe({
          next: (data) => {
            Swal.fire('Success', 'Asset has been successfully deleted', 'success')
                .then(() => {
                  let filtered = this.assets.filter((el) => { 
                    return el.assetId != assetId; 
                  }); 

                  this.assets = filtered;
                });
          }
        });
      }
    })
  }

  openAddForm(template: TemplateRef<any>) {
    this.isAddForm = true;
    this.activeAssetId = 0;

    this.openModal(template);
  }

  openEditForm(template: TemplateRef<any>, asset: Asset) {
    this.isAddForm = false;
    this.activeAssetId = asset.assetId as unknown as number;

    this.assetForm.setValue({
      assetId: asset.assetId,
      name: asset.name,
      validFrom: asset.validFrom == 'NULL' ? null : asset.validFrom,
      validTo: asset.validTo     == 'NULL' ? null : asset.validTo,
      price: asset.price
    });

    this.openModal(template);
  }

  private openModal(template: TemplateRef<any>) {
    const _combine = combineLatest(
      this.modalService.onShow,
      this.modalService.onShown,
      this.modalService.onHide,
      this.modalService.onHidden
    ).subscribe(() => this.changeDetection.markForCheck());
 
    this.subscriptions.push(
      this.modalService.onHide.subscribe(() => {
        this.resetAssetFormState();

        this.assetForm.setValue({
          assetId: 0,
          name: '',
          validFrom: null,
          validTo: null,
          price: ''
        });
      })
    );
 
    this.subscriptions.push(_combine);
    this.modalRef = this.modalService.show(template);
  }
 
  unsubscribe() {
    this.subscriptions.forEach((subscription: Subscription) => {
      subscription.unsubscribe();
    });
    this.subscriptions = [];
  }

  private buildAssetForm(): void {
    const abstractControlOptions = {
      updateOn: 'submit'
    }

    this.assetForm = this.formBuilder.group(
      {
        assetId: [0],
        name: ['', [Validators.required]],
        validFrom: [null],
        validTo: [null],
        price: ['', [Validators.required]]
      },
      abstractControlOptions as AbstractControlOptions
    );
  }

  submitForm(): void {
    if (!this.validateAssetForm()) {
      Swal.fire('Error', 'Please provide details for the required fields.', 'error');
      return;
    } 

    if (this.isAddForm) {
      this.assetService
          .addAsset(this.assetForm.value)
          .subscribe({
            next: (data) => {
              console.log(data);

              let asset: Asset = this.assetForm.value;
              asset.assetId = data.assetId;
              asset.validFrom = asset.validFrom == null ? 'NULL' : new Date(asset.validFrom).toISOString().substring(0, 10);
              asset.validTo   = asset.validTo   == null ? 'NULL' : new Date(asset.validTo).toISOString().substring(0, 10);

              this.assets.push(asset);

              Swal.fire('Success', 'Asset has been added successfully', 'success')
                  .then(() => {
                    this.modalRef?.hide();
                  });
            }
          });
    } else {
        this.assetService
          .updateAsset(this.assetForm.value)
          .subscribe({
            next: () => {
              const newArr = this.assets.map(obj => {
                const x = this.assetForm.value;

                if (obj.assetId === x.assetId) {
                  return {
                    ...obj, 
                    name: x.name,
                    validFrom: x.validFrom == null ? 'NULL' : x.validFrom,
                    validTo: x.validTo == null ? 'NULL' : x.validTo,
                    price: x.price
                  };
                }
              
                return obj;
              });

              this.assets = newArr;

              Swal.fire('Success', 'Asset has been updated successfully', 'success')
                  .then(() => {
                    this.modalRef?.hide();
                  });
            }
          });
      }
    
  }

  private validateAssetForm() : boolean {
    if (this.assetForm.valid) 
      return true;

    else {
      if (this.assetForm.get('name')?.hasError('required'))
        this.isvalidName = false;

      if (this.assetForm.get('price')?.hasError('required'))
        this.isvalidPrice = false;
    }

    return false;
  }

  private resetAssetFormState() {
    this.isvalidPrice = true;
    this.isvalidName = true;
  }
}
