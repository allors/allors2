import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialSelectModule, AllorsMaterialInputModule, AllorsMaterialTextAreaModule } from '../../../..';


import { InvoiceOverviewComponent } from './invoice-overview.component';
export { InvoiceOverviewComponent } from './invoice-overview.component';

@NgModule({
  declarations: [
    InvoiceOverviewComponent,
  ],
  exports: [
    InvoiceOverviewComponent,
  ],
  imports: [
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,  
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class InvoiceOverviewModule {}
