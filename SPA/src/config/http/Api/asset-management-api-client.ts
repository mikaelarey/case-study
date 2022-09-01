import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { XHttpClient } from "../x-http-client";

export class AssetManagementApiClient extends XHttpClient {
  constructor(public override http: HttpClient) {
    super(http, environment.api.assetManagement);
  }
}

export function AssetManagementApiClientFactory(http: HttpClient): any {
  return new AssetManagementApiClient(http);
}

export const AssetManagementApiClientProvider = {
  provide: AssetManagementApiClient,
  useFactory: AssetManagementApiClientFactory,
  deps: [HttpClient]
}