import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { SalesOrderOverviewComponent } from "./salesOrderOverview.component";
export { SalesOrderOverviewComponent } from "./salesOrderOverview.component";

@NgModule({
  declarations: [
    SalesOrderOverviewComponent,
  ],
  exports: [
    SalesOrderOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class SalesOrderOverviewModule {}
