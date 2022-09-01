import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoutingModule } from './routing/routing.module';
import { XHttpClientModule } from './http/http.module';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RoutingModule,
    XHttpClientModule
  ],
  exports:[RoutingModule]
})
export class ConfigModule { }
