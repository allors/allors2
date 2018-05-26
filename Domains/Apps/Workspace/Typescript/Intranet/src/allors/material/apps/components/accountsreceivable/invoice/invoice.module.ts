import { NgModule } from '@angular/core';

import { InvoiceComponent } from './invoice.component';
import { FormsModule } from '@angular/forms';
export { InvoiceComponent } from './invoice.component';

@NgModule({
  declarations: [
    InvoiceComponent,
  ],
  exports: [
    InvoiceComponent,
  ],
  imports: [
    FormsModule,
  ],
})
export class InvoiceModule {}
