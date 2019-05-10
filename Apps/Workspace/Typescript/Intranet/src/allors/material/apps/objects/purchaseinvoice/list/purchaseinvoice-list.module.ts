import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatTableModule, MatSortModule, MatDialogModule, MatPaginatorModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import {
  AllorsMaterialHeaderModule, AllorsMaterialFileModule, AllorsMaterialFilterModule, AllorsMaterialInputModule, AllorsMaterialSelectModule,
  AllorsMaterialSideNavToggleModule, AllorsMaterialSlideToggleModule, AllorsMaterialStaticModule, AllorsMaterialTextAreaModule,
  AllorsMaterialFactoryFabModule, AllorsMaterialTableModule
} from '../../../../../material';

import { PurchaseInvoiceListComponent } from './purchaseinvoice-list.component';
export { PurchaseInvoiceListComponent } from './purchaseinvoice-list.component';

@NgModule({
  declarations: [
    PurchaseInvoiceListComponent,
  ],
  exports: [
    PurchaseInvoiceListComponent,
  ],
  imports: [
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFileModule,
    AllorsMaterialFilterModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTableModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDialogModule,
    MatDividerModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatPaginatorModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    MatTableModule,
    MatSortModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PurchaseInvoiceListModule { }
