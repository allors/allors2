import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatOptionModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
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
