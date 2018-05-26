import { NgModule } from '@angular/core';


import { InvoiceOverviewComponent } from './invoice-overview.component';
import { FormsModule } from '@angular/forms';
export { InvoiceOverviewComponent } from './invoice-overview.component';

@NgModule({
  declarations: [
    InvoiceOverviewComponent,
  ],
  exports: [
    InvoiceOverviewComponent,
    
  ],
  imports: [
    FormsModule,
  ],
})
export class InvoiceOverviewModule {}
