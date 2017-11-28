import { NgModule } from "@angular/core";
import { SharedModule } from "../../../shared.module";

import { InvoicesOverviewComponent } from "./invoices-overview.component";
export { InvoicesOverviewComponent } from "./invoices-overview.component";

@NgModule({
  declarations: [
    InvoicesOverviewComponent,
  ],
  exports: [
    InvoicesOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class InvoicesOverviewModule {}
