import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatTableModule, MatSortModule, MatDialogModule, MatPaginatorModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialHeaderModule } from '../../../../base/components/header';
import { AllorsMaterialFileModule } from '../../../../base/components/role/file';
import { AllorsMaterialFilterModule } from '../../../../base/components/filter';
import { AllorsMaterialInputModule } from '../../../../base/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/role/textarea';
import { AllorsMaterialFactoryFabModule } from '../../../../base/components/factoryfab/factoryfab.module';

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
