import { NgModule } from '@angular/core';


import { InvoicePrintComponent } from './invoice-print.component';
import { FormsModule } from '@angular/forms';
export { InvoicePrintComponent } from './invoice-print.component';

@NgModule({
  declarations: [
    InvoicePrintComponent,
  ],
  exports: [
    InvoicePrintComponent,
    
  ],
  imports: [
    FormsModule,
  ],
})
export class InvoicePrintModule {}
