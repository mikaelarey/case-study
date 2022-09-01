import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MainComponent } from "src/app/main/main.component";

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: '',
        redirectTo: 'assets',
        pathMatch: 'full'
      },
      {
        path: 'assets',
        loadChildren: async () => (await import('../../app/assets/assets.module')).AssetsModule
      },
      {
        path: 'invoices',
        loadChildren: async () => (await import('../../app/invoices/invoices.module')).InvoicesModule
      },
      {
        path: 'invoice-details',
        loadChildren: async () => (await (import('../../app/invoice-details/invoice-details.module'))).InvoiceDetailsModule
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'error'
  }
]

@NgModule({
    imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
    exports: [RouterModule]
})
export class RoutingModule {}

