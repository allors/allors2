import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { InvoicesOverviewComponent } from "./invoicesOverview.component";
export { InvoicesOverviewComponent } from "./invoicesOverview.component";

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
