import { NgModule } from '@angular/core';




import { RepeatingInvoiceEditComponent } from './repeatinginvoice.component';
import { FormsModule } from '@angular/forms';
export { RepeatingInvoiceEditComponent } from './repeatinginvoice.component';

@NgModule({
  declarations: [
    RepeatingInvoiceEditComponent,
  ],
  exports: [
    RepeatingInvoiceEditComponent,
  ],
  imports: [
    FormsModule
  ],
})
export class RepeatingInvoiceEditModule {}
