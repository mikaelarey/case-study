import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ModalModule } from 'ngx-bootstrap/modal';

import { AppComponent } from './app.component';
import { MainComponent } from './main/main.component';

import { ConfigModule } from 'src/config/config.module';
import { SharedModule } from './shared/shared.module';
import { AssetsComponent } from './assets/assets.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import {RouterModule} from '@angular/router';
import { InvoicesComponent } from './invoices/invoices.component';


@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    AssetsComponent,
    InvoicesComponent
  ],
  imports: [
    BrowserModule,
    ConfigModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    ModalModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
