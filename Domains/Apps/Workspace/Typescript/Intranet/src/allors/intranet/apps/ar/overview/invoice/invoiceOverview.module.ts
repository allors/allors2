import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { InvoiceOverviewComponent } from "./invoiceOverview.component";
export { InvoiceOverviewComponent } from "./invoiceOverview.component";

@NgModule({
  declarations: [
    InvoiceOverviewComponent,
  ],
  exports: [
    InvoiceOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class InvoiceOverviewModule {}
