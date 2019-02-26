import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule} from '../../../..';

import { PurchaseOrderListComponent } from './purchaseorder-list.component';
export { PurchaseOrderListComponent } from './purchaseorder-list.component';

@NgModule({
  declarations: [
    PurchaseOrderListComponent,
  ],
  exports: [
    PurchaseOrderListComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
  ],
})
export class PurchaseOrderListModule { }
