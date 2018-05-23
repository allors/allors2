import { NgModule } from '@angular/core';

import { InlineModule } from '../../inline.module';
import { SharedModule } from '../../shared.module';

import { InvoiceItemEditComponent } from './invoiceitem.component';
export { InvoiceItemEditComponent } from './invoiceitem.component';

@NgModule({
  declarations: [
    InvoiceItemEditComponent,
  ],
  exports: [
    InvoiceItemEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class InvoiceItemEditModule {}
