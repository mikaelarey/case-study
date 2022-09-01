import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, map, Observable, switchAll, throwError} from "rxjs";
import { Asset } from "src/app/shared/models/asset.model";
import { AssetRepositoryService } from "../repository/assets.repository";

import Swal from 'sweetalert2';

@Injectable({ providedIn: 'root' })
export class AssetService {
  constructor(
    private assetRepositoryService: AssetRepositoryService
  ) {}

  getAssets(): Observable<any> {
    return this.assetRepositoryService.getAssets().pipe(
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

  addAsset(asset: Asset): Observable<Asset> {
    return this.assetRepositoryService.addAsset(asset).pipe(
        catchError((error) => {
          this.setErrorMessages(error);
          return throwError(() => new Error(error));
        }),
        map((data) => {
            return data;
        })
      );
  }

  updateAsset(asset: Asset): Observable<Asset> {
    return this.assetRepositoryService.updateAsset(asset).pipe(
        catchError((error) => {
          this.setErrorMessages(error);
          return throwError(() => new Error(error));
        }),
        map((data) => {
            return data;
        })
      );
  }

  deleteAsset(assetId: number): Observable<any> {
    return this.assetRepositoryService.deleteAsset(assetId).pipe(
        catchError((error) => {
          this.setErrorMessages(error);
          return throwError(() => new Error(error));
        }),
        map((data) => {
            return data;
        })
      );
  }

  private setErrorMessages(error: any) {
    if (error.status == 409) Swal.fire(error.statusText, "The record you are trying to add is already exists.", "error");
    else if (error.status == 400) Swal.fire(error.statusText, "Invalid input. Please check your entries.", "error");
    else Swal.fire(error.statusText, "There was an error occured while processing your request", "error");
  }
}