import { NgModule } from '@angular/core';




import { InvoiceItemEditComponent } from './invoiceitem.component';
import { FormsModule } from '@angular/forms';
export { InvoiceItemEditComponent } from './invoiceitem.component';

@NgModule({
  declarations: [
    InvoiceItemEditComponent,
  ],
  exports: [
    InvoiceItemEditComponent,

    
    
  ],
  imports: [
    FormsModule
    
  ],
})
export class InvoiceItemEditModule {}
