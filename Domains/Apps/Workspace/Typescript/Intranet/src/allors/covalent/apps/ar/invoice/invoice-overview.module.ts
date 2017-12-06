import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { InvoiceOverviewComponent } from "./invoice-overview.component";
export { InvoiceOverviewComponent } from "./invoice-overview.component";

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
