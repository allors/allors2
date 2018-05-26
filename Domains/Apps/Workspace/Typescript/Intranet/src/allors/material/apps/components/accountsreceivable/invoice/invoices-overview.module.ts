import { NgModule } from '@angular/core';
import { InvoicesOverviewComponent } from './invoices-overview.component';
import { FormsModule } from '@angular/forms';
export { InvoicesOverviewComponent } from './invoices-overview.component';

@NgModule({
  declarations: [
    InvoicesOverviewComponent,
  ],
  exports: [
    InvoicesOverviewComponent,
  ],
  imports: [
    FormsModule
  ],
})
export class InvoicesOverviewModule {}
