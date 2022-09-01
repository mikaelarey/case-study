import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Asset } from "src/app/shared/models/asset.model";
import { AssetManagementApiClient } from "src/config/http/Api/asset-management-api-client";

@Injectable({ providedIn: 'root' })
export class AssetRepositoryService {
  constructor (
    private http: AssetManagementApiClient
  ) {}

  getAssets(): Observable<Asset[]> {
    return this.http.get('assets');
  }

  addAsset(asset: Asset): Observable<Asset> {
    return this.http.post('assets', asset);
  }

  updateAsset(asset: Asset): Observable<Asset> {
    return this.http.put('assets', asset);
  }

  deleteAsset(assetId: number): Observable<any> {
    return this.http.delete('assets/' + assetId);
  }
}
