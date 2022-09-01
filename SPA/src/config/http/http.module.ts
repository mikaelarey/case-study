import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AssetManagementApiClientProvider } from './Api/asset-management-api-client';

@NgModule({
  imports: [HttpClientModule],
  exports: [HttpClientModule],
  providers: [AssetManagementApiClientProvider]
})
export class XHttpClientModule { }
