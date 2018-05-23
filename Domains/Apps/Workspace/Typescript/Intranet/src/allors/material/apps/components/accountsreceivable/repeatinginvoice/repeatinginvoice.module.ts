import { NgModule } from '@angular/core';

import { InlineModule } from '../../inline.module';
import { SharedModule } from '../../shared.module';

import { RepeatingInvoiceEditComponent } from './repeatinginvoice.component';
export { RepeatingInvoiceEditComponent } from './repeatinginvoice.component';

@NgModule({
  declarations: [
    RepeatingInvoiceEditComponent,
  ],
  exports: [
    RepeatingInvoiceEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class RepeatingInvoiceEditModule {}
