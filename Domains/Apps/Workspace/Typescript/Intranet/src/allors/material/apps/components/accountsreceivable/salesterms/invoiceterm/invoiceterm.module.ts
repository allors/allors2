import { NgModule } from '@angular/core';




import { InvoiceTermEditComponent } from './invoiceterm.component';
import { FormsModule } from '@angular/forms';
export { InvoiceTermEditComponent } from './invoiceterm.component';

@NgModule({
  declarations: [
    InvoiceTermEditComponent,
  ],
  exports: [
    InvoiceTermEditComponent,
  ],
  imports: [
    FormsModule
  ],
})
export class InvoiceTermEditModule {}
